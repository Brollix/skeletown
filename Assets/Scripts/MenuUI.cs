using UnityEngine;

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

    public void StartGame()
    {
        Time.timeScale = 1f;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetGame();
        }
        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadScene("DungeonScene");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);

        if (NavigationManager.Instance != null && settingsMenuScript != null)
        {
            NavigationManager.Instance.SetDefaultSelection(settingsMenuScript.GetFirstSelectable());
        }
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        
        if (NavigationManager.Instance != null && startGameButton != null)
        {
            NavigationManager.Instance.SetDefaultSelection(startGameButton);
        }
    }

    public void OpenUpgrades()
    {
        mainMenuPanel.SetActive(false);
        upgradesPanel.SetActive(true);
        
        if (NavigationManager.Instance != null && upgradesFirstButton != null)
        {
            NavigationManager.Instance.SetDefaultSelection(upgradesFirstButton);
        }
    }

    public void CloseUpgrades()
    {
        upgradesPanel.SetActive(false);
        mainMenuPanel.SetActive(true);

        if (NavigationManager.Instance != null && startGameButton != null)
        {
            NavigationManager.Instance.SetDefaultSelection(startGameButton);
        }
    }

    public void OpenCredits()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);

        if (NavigationManager.Instance != null && creditsFirstButton != null)
        {
            NavigationManager.Instance.SetDefaultSelection(creditsFirstButton);
        }
    }

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
