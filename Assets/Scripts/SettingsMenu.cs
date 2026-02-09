using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject settingsPanel;

    [Header("Video Settings")]
    [SerializeField] private Toggle fullscreenToggle;

    [Header("Audio Settings")]
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Toggle sfxToggle;
    [SerializeField] private Slider sfxSlider;

    [Header("Optional (Gameplay Only)")]
    [SerializeField] private GameObject pauseMenu; // Only used in DungeonScene


    // Loads saved preferences and initializes UI listeners.
    private void Start()
    {
        // --- Video ---
        // Load fullscreen preference (default true = 1)
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = isFullscreen;
            fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        }
        // Apply immediately in case resolution changed or first run
        Screen.fullScreen = isFullscreen;


        // --- Audio: Music ---
        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 1f);
        bool musicMute = PlayerPrefs.GetInt("MusicMuted", 0) == 1; // 1 means muted

        if (musicSlider != null)
        {
            musicSlider.value = musicVol;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (musicToggle != null)
        {
            // Toggle shows "Music On" status. So if mute is true, toggle is OFF.
            musicToggle.isOn = !musicMute;
            musicToggle.onValueChanged.AddListener(ToggleMusic);
        }


        // --- Audio: SFX ---
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 1f);
        bool sfxMute = PlayerPrefs.GetInt("SFXMuted", 0) == 1;

        if (sfxSlider != null)
        {
            sfxSlider.value = sfxVol;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }

        if (sfxToggle != null)
        {
            // Toggle shows "SFX On" status. So if mute is true, toggle is OFF.
            sfxToggle.isOn = !sfxMute; 
            sfxToggle.onValueChanged.AddListener(ToggleSFX);
        }


        // Keep panel hidden at start
        settingsPanel.SetActive(false);
    }


    // Shows the settings panel and hides the pause menu if active.
    public void OpenSettings()
    {
        // If the pause menu exists (DungeonScene), hide it when entering settings
        if (pauseMenu != null && PauseManager.GamePaused)
            pauseMenu.SetActive(false);

        settingsPanel.SetActive(true);
    }


    // Hides the settings panel and restores the pause menu.
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);

        // If closing from gameplay, show the pause menu again
        if (pauseMenu != null && PauseManager.GamePaused)
            pauseMenu.SetActive(true);
    }

    // --- Video Logic ---

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        // Save preference: 1 for true, 0 for false
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }


    // --- Audio Logic ---

    public void SetMusicVolume(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(value);
        }
    }

    public void ToggleMusic(bool isOn)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ToggleMusic(isOn);
        }
    }

    public void SetSFXVolume(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(value);
        }
    }

    public void ToggleSFX(bool isOn)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ToggleSFX(isOn);
        }
    }
}

