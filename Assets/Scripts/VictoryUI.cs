using UnityEngine;
// using UnityEngine.SceneManagement;

public class VictoryUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject victoryPanel;

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
            Time.timeScale = 0f; // Pause game
            
            if (GameManager.Instance != null) GameManager.Instance.IsGameOver = true;
            PauseManager.GamePaused = true;
        }
        else
        {
            Debug.LogWarning("[VictoryUI] Victory Panel not assigned!");
        }
    }

    public void AcceptVictoryAndGoToMenu()
    {
        Time.timeScale = 1f; // Unpause
        PauseManager.GamePaused = false;
        if (GameManager.Instance != null) GameManager.Instance.IsGameOver = false;

        // 1. Reset Upgrades (if manager exists)
        if (UpgradeManager.Instance != null)
        {
            UpgradeManager.Instance.ResetAllUpgrades();
        }

        // 2. Reset Progression (Level, XP, SkillPoints)
        if (PlayerExperience.Instance != null)
        {
            // Reset in-memory values if we had a method for it, 
            // but for now let's modify PlayerPrefs directly and force a reload/save if possible.
            // Since we don't have a direct "Reset" method in PlayerExperience, 
            // we will wipe the keys it uses.
            PlayerPrefs.DeleteKey("PlayerLevel");
            PlayerPrefs.DeleteKey("PlayerXP");
            PlayerPrefs.DeleteKey("SkillPoints");
            PlayerPrefs.Save();
            
            // Re-load to update the instance
            PlayerExperience.Instance.LoadProgress();
        }

        // 3. Load Main Menu
        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadScene("MainMenu");
        }
    }
}
