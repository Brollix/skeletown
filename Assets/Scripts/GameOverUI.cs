using UnityEngine;
// using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject gameOverPanel;

    [Header("Navigation")]
    [SerializeField] private GameObject mainMenuButton;


    // Subscribes to player death events and hides panel.
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
    }


    // Unsubscribes from player death events.
    private void OnDestroy()
    {
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.OnDeath -= ShowGameOver;
        }
    }


    // Pauses game and shows defeat screen.
    private void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f; // Pause the game
            
            if (GameManager.Instance != null) GameManager.Instance.IsGameOver = true;
            PauseManager.GamePaused = true;

            if (UnityEngine.EventSystems.EventSystem.current != null)
                UnityEngine.EventSystems.EventSystem.current.sendNavigationEvents = true;

            if (NavigationManager.Instance != null && mainMenuButton != null)
            {
                NavigationManager.Instance.SetDefaultSelection(mainMenuButton);
            }

            // Hide player to simulate death
            PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
            if (playerHealth != null)
            {
                SpriteRenderer sr = playerHealth.GetComponent<SpriteRenderer>();
                if (sr != null) sr.enabled = false;
            }

            // Hide Bow
            BowController bow = FindObjectOfType<BowController>();
            if (bow != null)
            {
                bow.gameObject.SetActive(false);
            }
        }
    }


    // Resumes time and returns to main menu.
    public void AcceptAndGoToMenu()
    {
        Time.timeScale = 1f; // Resume time before changing scene
        PauseManager.GamePaused = false;
        if (GameManager.Instance != null) GameManager.Instance.IsGameOver = false;

        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadScene("MainMenu");
        }
    }
}