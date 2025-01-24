using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public enum ItemType
    {
        Goldfish,
        Plant
    }

    public GameObject fish;
    public GameObject plant;
    public ItemType itemType;
    public float wobbliness = 0.01F;
    private Rigidbody2D _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        switch (itemType) {
            case ItemType.Goldfish:
                GetComponentInChildren<SpriteRenderer>().color = new Color(1.0f, 1.0f, 0.0f, 0.1f);
                break;
            case ItemType.Plant:
                GetComponentInChildren<SpriteRenderer>().color = new Color(0.5f, 1.0f, 0.5f, 0.1f);
                break;
            default:
                Debug.LogError("no such item");
                return;

        }
    }

    // Update is called once per frame
    void Update()
    {
        _rigidbody.AddForce(new Vector2(Random.Range(-wobbliness, wobbliness), 0.0f), ForceMode2D.Impulse);

        if (_rigidbody.position.y > 2 * Screen.height)
        {
            Destroy(gameObject);
        }
    }

    void OnMouseDown()
    {
        var v3 = Input.mousePosition;
        v3.z = 10.0F;
        v3 = Camera.main.ScreenToWorldPoint(v3);
        GameObject obj;
        switch (itemType)
        {
            case ItemType.Goldfish:
                obj = fish;
                break;
            case ItemType.Plant:
                obj = plant;
                break;
            default:
                Debug.LogError("no such item");
                return;
        }
        GameObject instance = Instantiate(obj, v3, Quaternion.identity);

        Destroy(gameObject);
    }
}
