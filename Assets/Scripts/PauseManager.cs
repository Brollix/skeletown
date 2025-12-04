using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public static bool GamePaused = false;

    [Header("UI Panels")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private InputActionReference pauseInput;

    private void Start()
    {
        GamePaused = false;
        Time.timeScale = 1f;

        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        if (settingsMenu != null)
            settingsMenu.SetActive(false);
    }

    private void OnEnable()
    {
        pauseInput.action.performed += togglePause;
    }

    private void OnDisable()
    {
        pauseInput.action.performed -= togglePause;
    }

    private void Update()
    {
        // Don't allow pause if settings menu is open
        if (settingsMenu != null && settingsMenu.activeSelf)
            return;


    }

    private void togglePause(InputAction.CallbackContext ctx)
    {

        if (GamePaused)
            ResumeGame();
        else
            PauseGame();

    }

    public void PauseGame()
    {
        GamePaused = true;

        if (pauseMenu != null) pauseMenu.SetActive(true);

        Time.timeScale = 0f;

        if (EventSystem.current != null)
            EventSystem.current.sendNavigationEvents = true;
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

    // Return to Main Menu using SceneLoader to preserve Managers
    public void GoToMainMenu()
    {
        GamePaused = false;
        Time.timeScale = 1f;

        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadMainMenu();
        }
        else
        {
            // Fallback if SceneLoader is missing (shouldn't happen if started from Boot)
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }
}
