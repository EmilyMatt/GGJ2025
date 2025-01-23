using UnityEngine;

public class FishController : MonoBehaviour
{
    public Vector4 aquariumRanges;
    private float _delay;
    private Rigidbody2D _rigidbody;

    private Vector3? _targetPoint;

    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (_targetPoint.HasValue)
        {
            var distanceToTarget = Vector3.Distance(transform.position, _targetPoint.Value);
            if (Vector3.Distance(transform.position, _targetPoint.Value) < 0.1f)
            {
                _targetPoint = null;
                _delay -= Time.deltaTime;
            }
            else
            {
                _rigidbody.MovePosition(Vector3.MoveTowards(transform.position, _targetPoint.Value, Time.deltaTime));
            }
        }
        else
        {
            _targetPoint = new Vector3(Random.Range(aquariumRanges.x, aquariumRanges.y),
                Random.Range(aquariumRanges.z, aquariumRanges.w), 0);
            _delay = 2;
        }
    }
}