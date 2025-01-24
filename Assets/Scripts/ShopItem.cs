using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public GameObject fish;
    public float wobbliness = 0.01F;
    private Rigidbody2D _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _rigidbody.AddForce(new Vector2(Random.Range(-wobbliness, wobbliness), 0.0f), ForceMode2D.Impulse);

        if (_rigidbody.position.y > 2 * Screen.height) {
            Destroy(gameObject);
        }
    }

    void OnMouseDown()
    {
        var v3 = Input.mousePosition;
        v3.z = 10.0F;
        v3 = Camera.main.ScreenToWorldPoint(v3);
        GameObject instance = Instantiate(fish, v3, Quaternion.identity);

        Destroy(gameObject);
    }
}
