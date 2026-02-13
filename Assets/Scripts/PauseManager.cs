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

    [Header("Navigation")]
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private SettingsMenu settingsMenuScript;

    private void Start()
    {
        GamePaused = false;

        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        if (settingsMenu != null)
            settingsMenu.SetActive(false);

        if (settingsMenuScript != null)
        {
            settingsMenuScript.OnClose.AddListener(CloseSettings);
        }
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
    }

    private void togglePause(InputAction.CallbackContext ctx)
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

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

    public void PauseGame()
    {
        GamePaused = true;

        if (pauseMenu != null) pauseMenu.SetActive(true);

        if (EventSystem.current != null)
            EventSystem.current.sendNavigationEvents = true;

        if (NavigationManager.Instance != null && resumeButton != null)
        {
            NavigationManager.Instance.SetDefaultSelection(resumeButton);
        }
    }

    public void ResumeGame()
    {
        GamePaused = false;

        if (pauseMenu != null) pauseMenu.SetActive(false);
    }

    public void OpenSettings()
    {
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (settingsMenu != null) 
        {
            settingsMenu.SetActive(true);
            
            if (NavigationManager.Instance != null && settingsMenuScript != null)
            {
                NavigationManager.Instance.SetDefaultSelection(settingsMenuScript.GetFirstSelectable());
            }
        }
    }

    public void CloseSettings()
    {
        if (settingsMenu != null) settingsMenu.SetActive(false);
        if (pauseMenu != null) 
        {
            pauseMenu.SetActive(true);
            
            if (NavigationManager.Instance != null && resumeButton != null)
            {
                NavigationManager.Instance.SetDefaultSelection(resumeButton);
            }
        }
    }

    public void GoToMainMenu()
    {
        GamePaused = false;

        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadScene("MainMenu");
        }
    }
}
