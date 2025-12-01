using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static bool GamePaused = false;

    [Header("UI Panels")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;

    private void Start()
    {
        GamePaused = false;
        Time.timeScale = 1f;

        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        if (settingsMenu != null)
            settingsMenu.SetActive(false);
    }

    private void Update()
    {
        if (settingsMenu != null && settingsMenu.activeSelf)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        GamePaused = true;
        if (pauseMenu != null) pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        if (UnityEngine.EventSystems.EventSystem.current != null)
            UnityEngine.EventSystems.EventSystem.current.sendNavigationEvents = true;
    }

    public void ResumeGame()
    {
        GamePaused = false;
        if (pauseMenu != null) pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OpenSettings()
    {
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (settingsMenu != null) settingsMenu.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsMenu != null) settingsMenu.SetActive(false);
        if (pauseMenu != null) pauseMenu.SetActive(true);
    }

    // Use GameFlowManager instead of SceneManager directly
    public void GoToMainMenu()
    {
        GamePaused = false;
        Time.timeScale = 1f;

        if (GameFlowManager.Instance != null)
        {
            GameFlowManager.Instance.ReturnToMainMenu();
        }
        else
        {
            Debug.LogError("PauseManager: No GameFlowManager found!");
        }
    }

}
