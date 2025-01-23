using UnityEngine;
using Random = UnityEngine.Random;

public class FishController : MonoBehaviour
{
    public Vector4 aquariumRanges;
    public float fadeMultiplier = 0.5f;
    public Vector3 targetPoint;
    public float speed;
    private float _delay;
    private Transform _foodItem;
    private Rigidbody2D _rigidbody;
    private TargetType _targetType;

    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.AddForce(Vector2.down, ForceMode2D.Impulse);
        _delay = 1;
    }

    // Update is called once per frame
    private void Update()
    {
        switch (_targetType)
        {
            case TargetType.Idle:
            {
                // TODO: Check hunger state here?
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

    private void ChooseNewIdleTarget()
    {
        _targetType = TargetType.Idle;
        targetPoint = new Vector3(Random.Range(aquariumRanges.x, aquariumRanges.y),
            Random.Range(aquariumRanges.z, aquariumRanges.w), 0);
        _delay = 2;
    }

    private void HandleChaseFood()
    {
        // Fish moves faster towards food
        _rigidbody.AddForce(speed * 1.5f * (_foodItem.position - transform.position));
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

    private enum TargetType
    {
        None,
        Idle,
        Food
    }

    private enum FishState
    {
        Chillin,
        Hungry,
        Sick
    }
}