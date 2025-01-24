using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public float generationTime;
    public float foodShootingSpeed = 2.0f;  
    public float foodShootingSidewaysSpeed = 0.5f;
    public GameObject food;

    private new Collider2D collider;
    private float _lastFoodGenerated = 0.0F;


    void Start()
    {
        collider = gameObject.GetComponent<Collider2D>();
    }

    void Update()
    {
        if (_lastFoodGenerated + generationTime < Time.fixedTime) {
            _lastFoodGenerated = Time.fixedTime;
            float width = collider.bounds.size.x;
            float height = collider.bounds.size.y;
            Vector3 v3 = new Vector3(transform.position.x + 0.5f * Random.Range(-width, width), transform.position.y + height, 0.0F);
            GameObject instance = Instantiate(food, v3, Quaternion.identity);
            instance.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-foodShootingSidewaysSpeed, foodShootingSidewaysSpeed), foodShootingSpeed), ForceMode2D.Impulse);
        }

    }
}
