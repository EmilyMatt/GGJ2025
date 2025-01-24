using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Canvas canvas;
    public GameObject food;
    public float feedingDelay;
    private Coroutine _feedTimer;
    private bool _readyToFeed = true;

    private void Start()
    {
        if (!canvas) canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    private void OnMouseDown()
    {
        var mousePressed = Input.GetMouseButtonDown(0);
        if (_readyToFeed && mousePressed)
        {
            DropFood();
            if (_feedTimer != null) StopCoroutine(_feedTimer);
            // Store it so it can be cancelled if needed
            _feedTimer = StartCoroutine(StartFeedTimer());
        }
        else if (mousePressed)
        {
            FishScareBubble();
        }
    }

    private void DropFood()
    {
        _readyToFeed = false;

        var mousePos = Input.mousePosition;
        mousePos.z = 10.0F;
        if (Camera.main) mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Instantiate(food, mousePos, Quaternion.identity);
    }

    private void FishScareBubble()
    {
    }

    private IEnumerator StartFeedTimer()
    {
        yield return new WaitForSeconds(feedingDelay);
        _readyToFeed = true;
        _feedTimer = null;
    }
}