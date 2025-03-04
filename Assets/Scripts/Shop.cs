using UnityEngine;
using Random = UnityEngine.Random;

public class Shop : MonoBehaviour
{
    public float margin = 10.0F;
    public float generationTime;
    public GameObject shopItem;
    private float _internalGenerationTime;

    private float _lastShopItemGenerated;


    private void Start()
    {
        _internalGenerationTime = Random.Range(generationTime * 0.9f, generationTime * 1.1f);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!Camera.main || !(_lastShopItemGenerated + _internalGenerationTime < Time.fixedTime)) return;
        _lastShopItemGenerated = Time.fixedTime;
        _internalGenerationTime = Random.Range(generationTime * 0.9f, generationTime * 1.1f);

        var pos = new Vector3(Random.Range(margin, Screen.width - margin), 20.0F, 10.0F);
        var v3 = Camera.main.ScreenToWorldPoint(pos);

        var instance = Instantiate(shopItem, v3, Quaternion.identity);
        var item = instance.GetComponent<ShopItem>();
        item.itemType = Random.Range(0, 10) switch
        {
            < 1 => ShopItem.ItemType.PurpleFish,
            < 4 => ShopItem.ItemType.Goldfish,
            < 6 => ShopItem.ItemType.Poisionplant,
            _ => ShopItem.ItemType.Plant
        };
    }
}