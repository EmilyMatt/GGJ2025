using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public GameObject pollutionPanel;
    public TextMeshProUGUI pollutionText;
    public GameObject[] fishObjects;
    public float pollutionLevel;
    public float pollutionLevelHigh;
    public float pollutionLevelGameOver;
    public GameObject gameOverPanel;

    private void Awake()
    {
        _instance = this;
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

    private void MaybeTogglePollutionPanel()
    {
        pollutionPanel.SetActive(pollutionLevel >= pollutionLevelHigh);
    }

    public void Pollute(float amount)
    {
        pollutionLevel = Mathf.Clamp(pollutionLevel + amount, 0, 100);

        MaybeTogglePollutionPanel();
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
        pollutionText.text = $"Pollution Level: {pollutionLevel:F2}%"; // Display with 2 decimal places
    }
}