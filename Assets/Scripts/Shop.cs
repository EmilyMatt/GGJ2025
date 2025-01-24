using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public float margin = 10.0F;
    public float generationTime;
    public GameObject shopItem;

    private float _lastShopItemGenerated = 0.0F;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (_lastShopItemGenerated + generationTime < Time.fixedTime) {
            _lastShopItemGenerated = Time.fixedTime;
            Vector3 pos = new Vector3(Random.Range(margin, Screen.width - margin), 20.0F, 10.0F);
            Vector3 v3 = Camera.main.ScreenToWorldPoint(pos);
            GameObject instance = Instantiate(shopItem, v3, Quaternion.identity);
        }
    }
}
