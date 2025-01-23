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
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out Vector2 localMousePosition);
        Vector3 mousePosition = canvas.transform.TransformPoint(localMousePosition);
        Debug.Log(string.Format("{0} {1}", localMousePosition, mousePosition));
        Instantiate(food, mousePosition, Quaternion.identity, canvas.transform);
    }
}
