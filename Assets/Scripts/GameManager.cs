using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager: MonoBehaviour
{

    public GameObject[] fishObjects;
    public GameObject gameOverPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
