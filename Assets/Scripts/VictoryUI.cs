using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject victoryPanel;

    private static bool hasShownVictory = false; // Static to prevent multiple calls

    private void Start()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
        
        // Reset the static flag when scene loads
        hasShownVictory = false;
    }

    public void ShowVictory()
    {
        if (hasShownVictory)
        {
            Debug.LogWarning("[VictoryUI] Victory already shown, ignoring duplicate call.");
            return;
        }

        // Check if player is dead - if so, don't show victory
        if (PlayerHealth.Instance != null && PlayerHealth.Instance.IsDead)
        {
            Debug.Log("[VictoryUI] Player is dead, not showing victory.");
            return;
        }

        hasShownVictory = true;

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            Time.timeScale = 0f; // Pause the game
            Debug.Log("[VictoryUI] Victory screen shown!");
        }
        else
        {
            Debug.LogWarning("[VictoryUI] Victory panel not assigned!");
        }
    }

    public void AcceptVictoryAndGoToMenu()
    {
        // Unpause before scene change
        Time.timeScale = 1f;

        // Reset upgrades
        if (UpgradeManager.Instance != null)
        {
            UpgradeManager.Instance.ResetAllUpgrades();
            UpgradeManager.Instance.SaveUpgrades();
        }

        // Reset player XP/level/skill points
        PlayerPrefs.SetInt("Level", 1);
        PlayerPrefs.SetFloat("XP", 0f);
        PlayerPrefs.SetInt("SkillPoints", 0);
        PlayerPrefs.Save();

        // Reset PlayerExperience
        if (PlayerExperience.Instance != null)
        {
            try
            {
                PlayerExperience.Instance.SaveProgress();
                PlayerExperience.Instance.LoadProgress();
            }
            catch { }
        }

        // Load main menu
        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadMainMenu();
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
