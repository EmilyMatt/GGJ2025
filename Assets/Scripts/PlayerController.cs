using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Canvas canvas;
    public GameObject food;
    private readonly bool _readyToFeed = true;

    private void Start()
    {
        if (!canvas) canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    private void OnMouseDown()
    {
        if (_readyToFeed)
        {
            DropFood();
        }
        else
        {
            FishScareBubble();
        }
    }


    private void DropFood()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = 10.0F;
        if (Camera.main) mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Instantiate(food, mousePos, Quaternion.identity);
    }

    private void FishScareBubble()
    {
        
    }
}