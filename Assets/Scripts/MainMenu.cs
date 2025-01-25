using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject helpPanel;
    private bool _showHelp;

    public void StartGame()
    {
        SceneManager.LoadScene("Aquarium");
    }

    public void ToggleHowToPlay()
    {
        _showHelp = !_showHelp;
        helpPanel.SetActive(_showHelp);
    }
}