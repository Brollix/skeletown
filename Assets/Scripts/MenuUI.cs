using UnityEngine;
// using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject upgradesPanel;
    public GameObject creditsPanel;


    // Initializes panel visibility on startup.
    private void Start()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        upgradesPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }


    // Resets game state and loads the main dungeon scene.
    public void StartGame()
    {
        Time.timeScale = 1f; // Ensure game is unpaused
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetGame();
        }
        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadScene("DungeonScene");
        }
    }


    // Closes the application.
    public void QuitGame()
    {
        Debug.Log("Quit pressed");
        Application.Quit();
    }


    // Switches to the settings panel.
    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }


    // Closes settings and returns to main menu.
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }


    // Switches to the upgrades panel.
    public void OpenUpgrades()
    {
        mainMenuPanel.SetActive(false);
        upgradesPanel.SetActive(true);
    }


    // Closes upgrades and returns to main menu.
    public void CloseUpgrades()
    {
        upgradesPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }


    // Switches to the credits panel.
    public void OpenCredits()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }


    // Closes credits and returns to main menu.
    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
