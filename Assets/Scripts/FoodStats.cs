using UnityEngine;

public class FoodStats : MonoBehaviour
{
    public float foodAmount;
    private SpriteRenderer _renderer;
    private float _timeOnGround;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!(transform.position.y < -3))
        {
            Debug.Log($"Food Height: {transform.position.y}");
            return;
        }


        _timeOnGround += Time.deltaTime;
        // Slowly reduce opacity
        if (_timeOnGround > 4)
            _renderer.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b,
                Mathf.Max(0, _renderer.color.a - 10));

        if (_timeOnGround > 7) Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
        }
    }
}