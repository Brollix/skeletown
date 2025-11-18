using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject gameOverPanel;

    private void Start()
    {
        // Subscribe to player death event
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.OnDeath += ShowGameOver;
        }

        // Hide game over panel initially
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
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

    public void AcceptAndGoToMenu()
    {
        Time.timeScale = 1f; // Resume time before changing scene
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