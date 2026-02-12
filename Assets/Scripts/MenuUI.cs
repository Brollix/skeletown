using UnityEngine;
// using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject upgradesPanel;
    public GameObject creditsPanel;

    [Header("Navigation")]
    [SerializeField] private GameObject startGameButton;
    [SerializeField] private GameObject upgradesFirstButton;
    [SerializeField] private GameObject creditsFirstButton;
    [SerializeField] private SettingsMenu settingsMenuScript;


    // Initializes panel visibility on startup.
    private void Start()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        upgradesPanel.SetActive(false);
        creditsPanel.SetActive(false);

        if (settingsMenuScript != null)
        {
            settingsMenuScript.OnClose.AddListener(CloseSettings);
        }

        if (NavigationManager.Instance != null && startGameButton != null)
        {
            NavigationManager.Instance.SetDefaultSelection(startGameButton);
        }
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

        if (NavigationManager.Instance != null && settingsMenuScript != null)
        {
            NavigationManager.Instance.SetDefaultSelection(settingsMenuScript.GetFirstSelectable());
        }
    }


    // Closes settings and returns to main menu.
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        
        if (NavigationManager.Instance != null && startGameButton != null)
        {
            NavigationManager.Instance.SetDefaultSelection(startGameButton);
        }
    }


    // Switches to the upgrades panel.
    public void OpenUpgrades()
    {
        mainMenuPanel.SetActive(false);
        upgradesPanel.SetActive(true);
        
        if (NavigationManager.Instance != null && upgradesFirstButton != null)
        {
            NavigationManager.Instance.SetDefaultSelection(upgradesFirstButton);
        }
    }


    // Closes upgrades and returns to main menu.
    public void CloseUpgrades()
    {
        upgradesPanel.SetActive(false);
        mainMenuPanel.SetActive(true);

        if (NavigationManager.Instance != null && startGameButton != null)
        {
            NavigationManager.Instance.SetDefaultSelection(startGameButton);
        }
    }


    // Switches to the credits panel.
    public void OpenCredits()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);

        if (NavigationManager.Instance != null && creditsFirstButton != null)
        {
            NavigationManager.Instance.SetDefaultSelection(creditsFirstButton);
        }
    }


    // Closes credits and returns to main menu.
    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);

        if (NavigationManager.Instance != null && startGameButton != null)
        {
            NavigationManager.Instance.SetDefaultSelection(startGameButton);
        }
    }
}
