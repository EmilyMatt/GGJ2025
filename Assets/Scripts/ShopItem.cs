using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public enum ItemType
    {
        Goldfish,
        Plant,
        Poisionplant
    }

    public GameObject fish;
    public GameObject plant;
    public GameObject poisionPlant;
    public ItemType itemType;
    public float wobbliness = 0.01F;
    private Rigidbody2D _rigidbody;

    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    private void Update()
    {
        _rigidbody.AddForce(new Vector2(Random.Range(-wobbliness, wobbliness), 0.0f), ForceMode2D.Impulse);

        if (transform.position.y > Screen.height * 1.1f) Destroy(gameObject);
    }

    private void OnMouseDown()
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
            case ItemType.Poisionplant:
                obj = poisionPlant;
                break;
            default:
                Debug.LogError("no such item");
                return;
        }

        Instantiate(obj, v3, Quaternion.identity);

        Destroy(gameObject);
    }
}