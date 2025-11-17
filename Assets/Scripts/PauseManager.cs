using UnityEngine;
using UnityEngine.SceneManagement;

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
        // Ignore ESC if settings menu is open
        if (settingsMenu != null && settingsMenu.activeSelf)
            return;

        // Toggle pause with ESC
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

        if (pauseMenu != null)
            pauseMenu.SetActive(true);

        // Freeze gameplay but allow UI interactions
        Time.timeScale = 0f;

        // Ensure EventSystem can still detect clicks
        UnityEngine.EventSystems.EventSystem.current.sendNavigationEvents = true;
    }

    public void ResumeGame()
    {
        GamePaused = false;

        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        Time.timeScale = 1f;
    }

    public void OpenSettings()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        if (settingsMenu != null)
            settingsMenu.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsMenu != null)
            settingsMenu.SetActive(false);

        if (pauseMenu != null)
            pauseMenu.SetActive(true);
    }

    public void GoToMainMenu()
    {
        GamePaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
