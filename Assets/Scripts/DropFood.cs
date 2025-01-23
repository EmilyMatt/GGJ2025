using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropFood : MonoBehaviour
{
    public Canvas canvas;
    public GameObject food;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnMouseDown() {
        var v3 = Input.mousePosition;
        v3.z = 10.0F;
        v3 = Camera.main.ScreenToWorldPoint(v3);
        GameObject instance = Instantiate(food, v3, Quaternion.identity);
    }
}
