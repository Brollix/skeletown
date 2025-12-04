using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject gameOverPanel;

    private static bool hasShownGameOver = false; // Static to prevent multiple calls

    private void Start()
    {
        // Subscribe to player death
        if (PlayerHealth.Instance != null)
        {
            PlayerHealth.Instance.OnDeath += ShowGameOver;
            Debug.Log("[GameOverUI] Subscribed to PlayerHealth.OnDeath");
        }
        else
        {
            StartCoroutine(WaitForPlayerHealth());
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        
        // Reset the static flag when scene loads
        hasShownGameOver = false;
    }

    private System.Collections.IEnumerator WaitForPlayerHealth()
    {
        while (PlayerHealth.Instance == null)
        {
            yield return null;
        }
        PlayerHealth.Instance.OnDeath += ShowGameOver;
        Debug.Log("[GameOverUI] Successfully subscribed to PlayerHealth.OnDeath");
    }

    private void ShowGameOver()
    {
        if (hasShownGameOver)
        {
            Debug.LogWarning("[GameOverUI] Game Over already shown, ignoring duplicate call.");
            return;
        }

        hasShownGameOver = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f; // Pause the game
            Debug.Log("[GameOverUI] Game Over screen shown!");
        }
    }

    public void AcceptAndGoToMenu()
    {
        Debug.Log("[GameOverUI] AcceptAndGoToMenu button clicked!");
        Time.timeScale = 1f; // Resume time before changing scene
        
        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadMainMenu();
        }
        else
        {
            Debug.LogError("[GameOverUI] CRITICAL: SceneLoader.Instance is NULL! Cannot load MainMenu safely!");
            Debug.LogError("[GameOverUI] This will destroy all managers! Please start from Boot scene!");
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from events
        if (PlayerHealth.Instance != null)
        {
            PlayerHealth.Instance.OnDeath -= ShowGameOver;
        }
    }
}
