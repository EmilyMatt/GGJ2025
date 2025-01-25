using UnityEngine;

public class DroppedFoodController : MonoBehaviour
{
    public float foodAmount;
    private SpriteRenderer _renderer;
    private float _timeOnGround;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (!(transform.position.y < -3)) return;


        _timeOnGround += Time.fixedDeltaTime;
        // Slowly reduce opacity
        if (_timeOnGround > 2)
        {
            var currentColor = _renderer.color;
            var newAlpha = GGJMathUtils.ConvertInRange(_timeOnGround, 2, 5, 1, 0.1f);
            _renderer.color = new Color(currentColor.r, currentColor.g, currentColor.b,
                Mathf.Max(0, newAlpha));
        }

        if (_timeOnGround > 5) Destroy(gameObject);
    }
}