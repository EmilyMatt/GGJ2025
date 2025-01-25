using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public PostProcessVolume postProcessVolume;
    public TextMeshProUGUI pollutionText;
    public GameObject[] fishObjects;
    public GameObject gameOverPanel;

    public float pollutionLevel;
    public float pollutionLevelHigh;
    public float pollutionLevelGameOver;

    private Vignette _vignette;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _vignette = postProcessVolume.profile.GetSetting<Vignette>();
    }

    private void FixedUpdate()
    {
        CheckForFish();
        UpdatePollutionUI();
    }

    public static GameManager GetInstance()
    {
        return _instance;
    }

    public void Pollute(float amount)
    {
        pollutionLevel = Mathf.Clamp(pollutionLevel + amount, 0, 100);
        if (pollutionLevel >= pollutionLevelGameOver) TriggerGameOver();
    }

    private void CheckForFish()
    {
        fishObjects = GameObject.FindGameObjectsWithTag("Fish");

        if (fishObjects.Length == 0) TriggerGameOver();
    }

    private void TriggerGameOver()
    {
        gameOverPanel?.SetActive(true);
        GameObject.Find("Tank").GetComponent<BoxCollider2D>().enabled = false;
        Time.timeScale = 0; // Pauses the game
    }

    public void RestartGame()
    {
        // Reset time scale (it was paused during Game Over)
        Time.timeScale = 1;

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    private void UpdatePollutionUI()
    {
        // Update the text to display the current pollution rate
        _vignette.color.value = new Color(0, GGJMathUtils.ConvertInRange(pollutionLevel, 0, 100, 0, 0.7f), 0, 1);
        pollutionText.text = $"Pollution Level: {pollutionLevel:F2}%"; // Display with 2 decimal places
    }
}