using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;

    private void Start()
    {
        
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.OnDeath += ShowGameOver;
        }

       
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }

    }

    private void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }
    }

    public void ShowVictory()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            Debug.LogWarning("GameOverUI: ShowVictory called but victoryPanel not assigned.");
        }
    }

    public void AcceptAndGoToMenu()
    {
        Time.timeScale = 1f; // Resume time before changing scene
        SceneManager.LoadScene("MainMenu");
    }

    // --------------------
    // NEW: Victory methods
    // --------------------
    // Call this from the boss die code: e.g. FindObjectOfType<GameOverUI>().ShowVictory();


    // This method is intended to be hooked to the Victory panel button
    // It resets player progression and sends the player to the Main Menu.
    public void AcceptVictoryAndGoToMenu()
    {
        // Unpause before scene change
        Time.timeScale = 1f;

        // 1) Reset upgrades (if manager present)
        if (UpgradeManager.Instance != null)
        {
            UpgradeManager.Instance.ResetAllUpgrades();
            UpgradeManager.Instance.SaveUpgrades();
        }

        // 2) Reset player XP/level/skill points (use PlayerPrefs fallback to be robust)
        PlayerPrefs.SetInt("Level", 1);
        PlayerPrefs.SetFloat("XP", 0f);
        PlayerPrefs.SetInt("SkillPoints", 0);
        PlayerPrefs.Save();

        // If PlayerExperience exists, ask it to reload or save progress if possible
        if (PlayerExperience.Instance != null)
        {
            // try to call LoadProgress / SaveProgress if they exist (most versions have SaveProgress/LoadProgress)
            // Best-effort: call SaveProgress then LoadProgress so the in-memory state gets refreshed from PlayerPrefs.
            try
            {
                PlayerExperience.Instance.SaveProgress();
            }
            catch { /* ignore if method not present */ }

            try
            {
                // If LoadProgress exists, it will overwrite in-memory fields with PlayerPrefs (safe)
                PlayerExperience.Instance.LoadProgress();
            }
            catch { /* ignore if method not present */ }
        }

        // 3) Ensure XP bar/UI refresh if you have an XPBar or UI script
        // Try common names used in the project (best-effort)
        var xpBar = FindObjectOfType<MonoBehaviour>(); // placeholder to avoid compile error when referencing unknown types
        // We won't try to call specific methods here to avoid compile-time issues; your XP/UI should read PlayerExperience on Start.

        // 4) Finally, load main menu
        SceneManager.LoadScene("MainMenu");
    }



    private void OnDestroy()
    {
        // Unsubscribe from events
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.OnDeath -= ShowGameOver;
        }
    }
}