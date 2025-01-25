using System.Collections;
using System.Linq;
using FishSettings;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishController : MonoBehaviour
{
    private static readonly int IsYoung = Animator.StringToHash("IsYoung");
    private static readonly int IsAdult = Animator.StringToHash("IsAdult");
    private static readonly int IsSick = Animator.StringToHash("IsSick");
    private static readonly int IsDead = Animator.StringToHash("IsDead");


    public AudioClip youngFishScream;
    public AudioClip fishScream;
    public AudioClip[] randomAudioHungry;

    public float fadeMultiplier = 0.5f;
    public Vector3 targetPoint;
    public float speed;
    public float hungerLevel;
    public FishState fishState;
    public TargetType targetType;
    public FishStats fishStats;
    private Animator _animator;
    private AudioSource _audioSource;

    private float _delay;
    private FishDirection _direction;
    private float _fishAge;
    private Transform _foodItem;
    private LifeStage _lifeStage;
    private CircleCollider2D _mouthCollider;
    private float _mouthOffset = 0.15f;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;


    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _mouthCollider = GetComponent<CircleCollider2D>();
        var spriteTransform = transform.Find("Sprite");
        _spriteRenderer = spriteTransform.GetComponent<SpriteRenderer>();
        _animator = spriteTransform.GetComponent<Animator>();

        // Give a little downward force so its not just static
        _rigidbody.AddForce(Vector2.down, ForceMode2D.Impulse);
        _delay = 1; // Give a second before moving away

        fishStats.aquariumRanges = PlayerController.GetInstance().aquariumRanges;
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (fishState == FishState.Dead) return;
        if (_lifeStage != LifeStage.Adult && fishState != FishState.Sick)
        {
            _fishAge += Time.fixedDeltaTime;
            CheckUpdateFishSize();
        }

        // use deltaTime so we can measure this in seconds
        hungerLevel -= Time.fixedDeltaTime;

        if (hungerLevel <= 0)
        {
            KillFish();
        }
        else if (hungerLevel < fishStats.starveThreshold)
        {
            fishState = FishState.Sick;
            //sound randowm from the hungry array
            _animator.SetBool(IsSick, true);
        }
        else
        {
            if (hungerLevel < fishStats.hungerThreshold)
            {
                // If we're moving into hungry state, play the sound
                if (fishState is FishState.Chillin or FishState.Sick) PlayRandomHungrySound();
                fishState = FishState.Hungry;
            }
            else
            {
                fishState = FishState.Chillin;
            }

            _animator.SetBool(IsSick, false);
        }

        switch (targetType)
        {
            case TargetType.Idle:
            {
                if ((fishState == FishState.Hungry) | (fishState == FishState.Sick))
                {
                    SearchForFoodItem();
                    if (_foodItem) break; // If no food was found, continue with Idle motion
                }

                HandleIdleMotion();
                break;
            }
            case TargetType.Food:
            {
                if (_foodItem)
                {
                    HandleChaseFood();
                    break;
                }

                ChooseNewIdleTarget();
                break;
            }
            case TargetType.None:
            {
                _delay -= Time.fixedDeltaTime;
                if (_delay <= 0) ChooseNewIdleTarget();
                break;
            }
        }
    }

    // Currently the only trigger we have is the mouth collision
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (fishState is FishState.Hungry or FishState.Sick && other.CompareTag("Food") &&
            other.transform.position.y > -3) EatFood(other.gameObject);
    }

    private void PlayScreamSound()
    {
        // Play the sound
        _audioSource.PlayOneShot(fishScream);
    }

    private void PlayRandomHungrySound()
    {
        var randomIndex = Random.Range(0, randomAudioHungry.Length);
        _audioSource.PlayOneShot(randomAudioHungry[randomIndex]);
    }

    private void EatFood(GameObject foodGameObject)
    {
        var foodStats = foodGameObject.GetComponent<DroppedFoodController>();
        hungerLevel = Mathf.Min(fishStats.maxHungerLevel, hungerLevel + foodStats.foodAmount);
        StartCoroutine(PolluteAquarium(foodStats.foodAmount * 0.15f, 2f));
        targetType = TargetType.None;
        _delay = 2;
        _rigidbody.AddForce(Vector2.down * 3);

        Destroy(foodGameObject);
    }


    private void CheckUpdateFishSize()
    {
        switch (_lifeStage)
        {
            case LifeStage.Beby when _fishAge > fishStats.youngThreshold:
                _mouthOffset = 0.25f;
                _lifeStage = LifeStage.Young;
                _animator.SetBool(IsYoung, true);
                fishScream = youngFishScream;
                break;
            case LifeStage.Young when _fishAge > fishStats.adultThreshold:
                _mouthOffset = 0.5f;
                _lifeStage = LifeStage.Adult;
                _animator.SetBool(IsAdult, true);
                break;
            default:
                return;
        }

        _mouthCollider.offset = new Vector2(_direction == FishDirection.Left ? -_mouthOffset : _mouthOffset, 0);
    }

    private void SearchForFoodItem()
    {
        // Yes I know its an expensive method but we need to always choose the closest food
        // TODO: maybe use a half-second interval between choosing closest food?
        var foodObject = GameObject.FindGameObjectsWithTag("Food").Select(x => x.transform)
            .Where(x => x.position.y > -3) // Don't waste time on food that will be gone in a sec
            .OrderBy(x => Vector3.Distance(transform.position, x.position)).FirstOrDefault();
        if (!foodObject) return;

        _foodItem = foodObject;
        targetType = TargetType.Food;
    }

    private void KillFish()
    {
        fishState = FishState.Dead;
        _spriteRenderer.flipY = true;
        _animator.SetBool(IsDead, true);
        PlayScreamSound();
        Destroy(gameObject, 5);
    }

    private void ChooseNewIdleTarget()
    {
        targetType = TargetType.Idle;
        targetPoint = new Vector3(Random.Range(fishStats.aquariumRanges.x, fishStats.aquariumRanges.y),
            Random.Range(fishStats.aquariumRanges.z, fishStats.aquariumRanges.w), 0);
        _delay = 2;
    }

    private void MaybeChangeDirection(Vector3 position)
    {
        var directionToFood = (position - transform.position).normalized;

        // Change direction if needed
        var shouldChangeDirection = (_direction == FishDirection.Left && directionToFood.x > 0) ||
                                    (_direction == FishDirection.Right && directionToFood.x < 0);
        if (!shouldChangeDirection) return;

        _direction = _direction == FishDirection.Left ? FishDirection.Right : FishDirection.Left;

        _spriteRenderer.flipX = _direction == FishDirection.Right; // Default sprite is left
        // Ensure mouth is on the correct side
        _mouthCollider.offset = new Vector2(-_mouthCollider.offset.x, _mouthCollider.offset.y);
    }

    private void HandleChaseFood()
    {
        var foodPosition = _foodItem.position;
        if (foodPosition.y < -3.0)
        {
            _foodItem = null;
            targetType = TargetType.None;
            return;
        }

        MaybeChangeDirection(foodPosition);

        // Add offset so food goes to mouth
        if (_direction == FishDirection.Left)
            foodPosition.x += _mouthOffset;
        else
            foodPosition.x -= _mouthOffset;

        // Fish moves faster towards food
        _rigidbody.MovePosition(Vector3.MoveTowards(transform.position, foodPosition,
            speed * Time.fixedDeltaTime * 10));
    }

    private void HandleIdleMotion()
    {
        MaybeChangeDirection(targetPoint);
        var distanceToTarget = Vector3.Distance(transform.position, targetPoint);
        if (distanceToTarget < 0.5f)
        {
            // Add force towards target for more realistic slowdown
            _rigidbody.AddForce((targetPoint - transform.position) * (speed * fadeMultiplier * _delay));
            if (_delay > 0)
                _delay -= Time.fixedDeltaTime;
            else
                ChooseNewIdleTarget();
        }
        else
        {
            _rigidbody.AddForce(speed * (targetPoint - transform.position));
        }
    }

    private IEnumerator PolluteAquarium(float level, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.GetInstance().Pollute(level);
    }
}