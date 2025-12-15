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


    // Resets pause state and UI on start.
    private void Start()
    {
        GamePaused = false;
        Time.timeScale = 1f;

        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        if (settingsMenu != null)
            settingsMenu.SetActive(false);
    }


    // Subscribes to the pause input action.
    private void OnEnable()
    {
        pauseInput.action.performed += togglePause;
    }


    // Unsubscribes from the pause input action.
    private void OnDisable()
    {
        pauseInput.action.performed -= togglePause;
    }

    private void Update()
    {
    }


    // Toggles between paused/resumed or closes settings if open.
    private void togglePause(InputAction.CallbackContext ctx)
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        // If settings menu is open, ESC should close it (go back) instead of unpausing
        if (settingsMenu != null && settingsMenu.activeSelf)
        {
            CloseSettings();
            return;
        }

        if (GamePaused)
            ResumeGame();
        else
            PauseGame();
    }


    // Freezes game time and shows the pause menu.
    public void PauseGame()
    {
        GamePaused = true;

        if (pauseMenu != null) pauseMenu.SetActive(true);

        Time.timeScale = 0f;

        if (EventSystem.current != null)
            EventSystem.current.sendNavigationEvents = true;
    }


    // Unfreezes game time and hides the pause menu.
    public void ResumeGame()
    {
        GamePaused = false;

        if (pauseMenu != null) pauseMenu.SetActive(false);

        Time.timeScale = 1f;
    }


    // Switches from pause menu to settings menu.
    public void OpenSettings()
    {
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (settingsMenu != null) settingsMenu.SetActive(true);
    }


    // Switches from settings menu back to pause menu.
    public void CloseSettings()
    {
        if (settingsMenu != null) settingsMenu.SetActive(false);
        if (pauseMenu != null) pauseMenu.SetActive(true);
    }

    // Return to Main Menu using SceneManager directly

    // Resumes time and loads the Main Menu scene.
    public void GoToMainMenu()
    {
        GamePaused = false;
        Time.timeScale = 1f;

        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadScene("MainMenu");
        }
    }
}
