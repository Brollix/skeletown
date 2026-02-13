using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject settingsPanel;
    public UnityEngine.Events.UnityEvent OnClose;

    [Header("Video Settings")]
    [SerializeField] private Toggle fullscreenToggle;

    [Header("Audio Settings")]
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Toggle sfxToggle;
    [SerializeField] private Slider sfxSlider;

    [Header("Optional (Gameplay Only)")]
    [SerializeField] private GameObject pauseMenu;

    private void Start()
    {
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = isFullscreen;
            fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        }
        Screen.fullScreen = isFullscreen;

        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 1f);
        bool musicMute = PlayerPrefs.GetInt("MusicMuted", 0) == 1;

        if (musicSlider != null)
        {
            musicSlider.value = musicVol;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (musicToggle != null)
        {
            musicToggle.isOn = !musicMute;
            musicToggle.onValueChanged.AddListener(ToggleMusic);
        }

        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 1f);
        bool sfxMute = PlayerPrefs.GetInt("SFXMuted", 0) == 1;

        if (sfxSlider != null)
        {
            sfxSlider.value = sfxVol;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }

        if (sfxToggle != null)
        {
            sfxToggle.isOn = !sfxMute; 
            sfxToggle.onValueChanged.AddListener(ToggleSFX);
        }

        settingsPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        if (pauseMenu != null && PauseManager.GamePaused)
            pauseMenu.SetActive(false);

        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);

        if (OnClose != null) OnClose.Invoke();

        if (pauseMenu != null && PauseManager.GamePaused)
            pauseMenu.SetActive(true);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        if (isFullscreen)
        {
            Resolution currentRes = Screen.currentResolution;
            Screen.SetResolution(currentRes.width, currentRes.height, true);
        }
        else
        {
            Screen.fullScreen = false;
        }

        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }

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

    public GameObject GetFirstSelectable()
    {
        return fullscreenToggle != null ? fullscreenToggle.gameObject : null;
    }
}
