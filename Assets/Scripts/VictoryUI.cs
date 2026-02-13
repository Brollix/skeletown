using UnityEngine;

public class VictoryUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject victoryPanel;

    [Header("Navigation")]
    [SerializeField] private GameObject mainMenuButton;

    private void Start()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
    }

    public void ShowVictory()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            Time.timeScale = 0f;
            
            if (GameManager.Instance != null) GameManager.Instance.IsGameOver = true;
            PauseManager.GamePaused = true;

            if (UnityEngine.EventSystems.EventSystem.current != null)
                UnityEngine.EventSystems.EventSystem.current.sendNavigationEvents = true;

            if (NavigationManager.Instance != null && mainMenuButton != null)
            {
                NavigationManager.Instance.SetDefaultSelection(mainMenuButton);
            }
        }
    }

    public void AcceptVictoryAndGoToMenu()
    {
        Time.timeScale = 1f;
        PauseManager.GamePaused = false;
        if (GameManager.Instance != null) GameManager.Instance.IsGameOver = false;

        if (UpgradeManager.Instance != null)
        {
            UpgradeManager.Instance.ResetAllUpgrades();
        }

        if (PlayerExperience.Instance != null)
        {
            PlayerPrefs.DeleteKey("PlayerLevel");
            PlayerPrefs.DeleteKey("PlayerXP");
            PlayerPrefs.DeleteKey("SkillPoints");
            PlayerPrefs.Save();
            
            PlayerExperience.Instance.LoadProgress();
        }

        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadScene("MainMenu");
        }
    }
}
