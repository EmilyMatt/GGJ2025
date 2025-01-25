using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager: MonoBehaviour
{
    public GameObject pollutionPanel;
    public GameObject[] fishObjects;
    public float pollutionLevel;
    public float pollutionLevelHigh;
    public float pollutionLevelGameOver;
    public GameObject gameOverPanel;
    private static GameManager _instance;
    public static GameManager GetInstance()
    {
        return _instance;
    }
    
    private void Awake()
    {
        _instance = this;
    }
    
    private void MaybeTogglePollutionPanel()
    {
        pollutionPanel.SetActive(pollutionLevel >= pollutionLevelHigh);
    }
    
    public void Pollute(float amount)
    {
        pollutionLevel = Mathf.Max(0, pollutionLevel + amount);
        pollutionLevel += amount;

        MaybeTogglePollutionPanel();
        if (pollutionLevel > pollutionLevelGameOver)
        {
            TriggerGameOver();
        }
    }

    void FixedUpdate()
    {
        CheckForFish();
    }

    void CheckForFish(){

        fishObjects = GameObject.FindGameObjectsWithTag("Fish");

        if (fishObjects.Length == 0){
            TriggerGameOver();
        }


    }

    void TriggerGameOver(){

        gameOverPanel?.SetActive(true);
        Time.timeScale = 0; // Pauses the game
    }

    public void RestartGame()
    {
        Debug.Log("Restart button clicked!");
        // Reset time scale (it was paused during Game Over)
        Time.timeScale = 1;

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
