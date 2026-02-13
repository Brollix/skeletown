using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject gameOverPanel;

    [Header("Navigation")]
    [SerializeField] private GameObject mainMenuButton;

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

    private void OnDestroy()
    {
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.OnDeath -= ShowGameOver;
        }
    }

    private void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
            
            if (GameManager.Instance != null) GameManager.Instance.IsGameOver = true;
            PauseManager.GamePaused = true;

            if (UnityEngine.EventSystems.EventSystem.current != null)
                UnityEngine.EventSystems.EventSystem.current.sendNavigationEvents = true;

            if (NavigationManager.Instance != null && mainMenuButton != null)
            {
                NavigationManager.Instance.SetDefaultSelection(mainMenuButton);
            }

            PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
            if (playerHealth != null)
            {
                SpriteRenderer sr = playerHealth.GetComponent<SpriteRenderer>();
                if (sr != null) sr.enabled = false;
            }

            BowController bow = FindObjectOfType<BowController>();
            if (bow != null)
            {
                bow.gameObject.SetActive(false);
            }
        }
    }

    public void AcceptAndGoToMenu()
    {
        Time.timeScale = 1f;
        PauseManager.GamePaused = false;
        if (GameManager.Instance != null) GameManager.Instance.IsGameOver = false;

        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadScene(GameConstants.SCENE_MAIN_MENU);
        }
    }
}