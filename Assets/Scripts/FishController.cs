using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishController : MonoBehaviour
{
    public enum FishState
    {
        Chillin,
        Hungry,
        Sick,
        Dead
    }

    public enum TargetType
    {
        None,
        Idle,
        Food
    }

    public Vector4 aquariumRanges;
    public float fadeMultiplier = 0.5f;
    public Vector3 targetPoint;
    public float starveThreshold = 5;
    public float hungerThreshold = 10;
    public float youngThreshold = 5;
    public float adultThreshold = 10;
    public float speed;
    public float maxHungerLevel;
    public float hungerLevel;
    public TargetType targetType;
    public FishState fishState;
    private float _delay;
    private FishDirection _direction;
    private float _fishAge;
    private Transform _foodItem;
    private LifeStage _lifeStage = LifeStage.Beby;
    private Rigidbody2D _rigidbody;


    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        // Give a little downward force so its not just static
        _rigidbody.AddForce(Vector2.down, ForceMode2D.Impulse);
        _delay = 1; // Give a second before moving away
    }

    // Update is called once per frame
    private void Update()
    {
        if (fishState == FishState.Dead) return;
        if (_lifeStage != LifeStage.Adult && fishState != FishState.Sick)
        {
            _fishAge += Time.deltaTime;
            CheckUpdateFishSize();
        }

        // use deltaTime so we can measure this in seconds
        hungerLevel -= Time.deltaTime;

        if (hungerLevel <= 0)
            KillFish();
        else if (hungerLevel < starveThreshold)
            // TODO: modify sprite to sick sprite
            fishState = FishState.Sick;
        else if (hungerLevel < hungerThreshold)
            fishState = FishState.Hungry;
        else
            fishState = FishState.Chillin;

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
                _delay -= Time.deltaTime;
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

    private void EatFood(GameObject foodGameObject)
    {
        var foodStats = foodGameObject.GetComponent<DroppedFoodController>();
        hungerLevel = Mathf.Min(maxHungerLevel, hungerLevel + foodStats.foodAmount);
        StartCoroutine(PolluteAquarium(foodStats.foodAmount, 2f));
        targetType = TargetType.None;
        _delay = 2;
        _rigidbody.AddForce(Vector2.down);

        Destroy(foodGameObject);
    }


    private void CheckUpdateFishSize()
    {
        switch (_lifeStage)
        {
            case LifeStage.Beby when _fishAge > youngThreshold:
                // TODO: Enlarge fish and collision, modify stats
                _lifeStage = LifeStage.Young;
                Debug.Log("Fish is young now");
                break;
            case LifeStage.Young when _fishAge > adultThreshold:
                // TODO: Enlarge fish and collision, modify stats
                _lifeStage = LifeStage.Adult;
                Debug.Log("Fish is adult now");
                break;
        }
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
        // TODO: modify sprite to dead sprite
        fishState = FishState.Dead;
    }

    private void ChooseNewIdleTarget()
    {
        targetType = TargetType.Idle;
        targetPoint = new Vector3(Random.Range(aquariumRanges.x, aquariumRanges.y),
            Random.Range(aquariumRanges.z, aquariumRanges.w), 0);
        _delay = 2;
    }

    private void ChangeDirection()
    {
        // TODO: Change sprite here, Change position of mouth collider
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

        var directionToFood = foodPosition - transform.position;
        switch (directionToFood.x, _direction)
        {
            case (> 0, FishDirection.Left):
                ChangeDirection();
                break;
            case (> 0, FishDirection.Right):
                // Real simple, just continue moving that way
                foodPosition.x -= 0.75f;
                break;
            case (< 0, FishDirection.Left):
                // Real simple, just continue moving that way
                foodPosition.x += 0.75f;
                break;
            case (< 0, FishDirection.Right):
                ChangeDirection();
                break;
            case (0, FishDirection.Left):
                foodPosition.x += 0.75f;
                break;
            case (0, FishDirection.Right):
                foodPosition.x -= 0.75f;
                break;
        }

        // TODO: Check if fish is facing target(if transform.position.x is pos or neg), and do animation
        // Fish moves faster towards food
        _rigidbody.MovePosition(Vector3.MoveTowards(transform.position, foodPosition,
            speed * Time.deltaTime * 15));
    }

    private void HandleIdleMotion()
    {
        // TODO: Check if fish is facing target(if transform.position.x is pos or neg), and do animation
        var distanceToTarget = Vector3.Distance(transform.position, targetPoint);
        if (distanceToTarget < 0.5f)
        {
            // Add force towards target for more realistic slowdown
            _rigidbody.AddForce((targetPoint - transform.position) * (speed * fadeMultiplier * _delay));
            if (_delay > 0)
                _delay -= Time.deltaTime;
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
        PlayerController.GetInstance().Pollute(level);
    }

    private enum FishDirection
    {
        Right,
        Left
    }

    private enum LifeStage
    {
        Beby,
        Young,
        Adult
    }
}