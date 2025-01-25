using UnityEngine;

public class Shop : MonoBehaviour
{
    public float margin = 10.0F;
    public float generationTime;
    public GameObject shopItem;

    private float _lastShopItemGenerated;

    // Update is called once per frame
    private void Update()
    {
        if (!Camera.main || !(_lastShopItemGenerated + generationTime < Time.fixedTime)) return;
        _lastShopItemGenerated = Time.fixedTime;

        var pos = new Vector3(Random.Range(margin, Screen.width - margin), 20.0F, 10.0F);
        var v3 = Camera.main.ScreenToWorldPoint(pos);

        var instance = Instantiate(shopItem, v3, Quaternion.identity);
        var item = instance.GetComponent<ShopItem>();
        item.itemType = Random.Range(0, 10) switch
        {
            < 4 => ShopItem.ItemType.Goldfish,
            < 7 => ShopItem.ItemType.Poisionplant,
            _ => ShopItem.ItemType.Plant
        };
    }
}