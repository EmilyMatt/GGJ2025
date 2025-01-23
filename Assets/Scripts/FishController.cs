using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishController : MonoBehaviour
{
    public enum FishState
    {
        Chillin,
        Hungry,
        Sick
    }

    public enum LifeStage
    {
        Beby,
        Young,
        Adult
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
    public float speed;
    public float hungerLevel;
    public TargetType targetType;
    public FishState fishState;
    private bool _alive = true;
    private float _delay;
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
        if (!_alive) return;

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
                HandleChaseFood();
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
        _alive = false;
    }

    private void ChooseNewIdleTarget()
    {
        targetType = TargetType.Idle;
        targetPoint = new Vector3(Random.Range(aquariumRanges.x, aquariumRanges.y),
            Random.Range(aquariumRanges.z, aquariumRanges.w), 0);
        _delay = 2;
    }

    private void HandleChaseFood()
    {
        // TODO: Check if fish is facing target(if transform.position.x is pos or neg), and do animation
        // Fish moves faster towards food
        _rigidbody.MovePosition(Vector3.MoveTowards(transform.position, _foodItem.position,
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
}