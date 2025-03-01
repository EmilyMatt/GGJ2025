using System.Collections;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController _instance;
    public Vector4 aquariumRanges;
    public Canvas canvas;
    public GameObject food;
    public float feedingDelay;
    public AudioClip foodDropSound;
    private AudioSource _audioSource;
    private Coroutine _feedTimer;
    private bool _readyToFeed = true;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        if (!canvas) canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        if (!Camera.main) return;

        var mousePressed = Input.GetMouseButtonDown(0);
        if (!_readyToFeed || !mousePressed) return;

        var mousePos = Input.mousePosition;
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

        if (ShouldDropFoodHere(mouseWorldPos))
        {
            DropFood(mouseWorldPos);
            PlayClickSound();
            if (_feedTimer != null) StopCoroutine(_feedTimer);
            // Store it so it can be cancelled if needed
            _feedTimer = StartCoroutine(StartFeedTimer());
        }

        // TODO: Play boop sound here
    }


    private static bool ShouldDropFoodHere(Vector3 mousePosition)
    {
        // No need to check over a direction(can use Vector2.zero)
        // as all elements are in the same position, will simply perform an overlap check at that position
        return !Physics2D.RaycastAll(mousePosition, Vector2.zero).Any(x => x.transform.root.name != "Tank");
    }

    private void PlayClickSound()
    {
        // Play the sound
        _audioSource.PlayOneShot(foodDropSound);
    }


    public static PlayerController GetInstance()
    {
        return _instance;
    }

    private void DropFood(Vector3 mouseWorldPos)
    {
        _readyToFeed = false;

        mouseWorldPos.z = 0.0F;
        Instantiate(food, mouseWorldPos, Quaternion.identity);
    }

    private IEnumerator StartFeedTimer()
    {
        yield return new WaitForSeconds(feedingDelay);
        _readyToFeed = true;
        _feedTimer = null;
    }
}