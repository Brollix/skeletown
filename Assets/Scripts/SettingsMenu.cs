using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Slider volumeSlider;

    [Header("Optional (Gameplay Only)")]
    [SerializeField] private GameObject pauseMenu; // Only used in DungeonScene

    private void Start()
    {
        // Load saved volume or default
        float vol = PlayerPrefs.GetFloat("MasterVolume", 1f);
        volumeSlider.value = vol;

        // Assign listener
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

        // Keep panel hidden at start
        settingsPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        // If the pause menu exists (DungeonScene), hide it when entering settings
        if (pauseMenu != null && PauseManager.GamePaused)
            pauseMenu.SetActive(false);

        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);

        // If closing from gameplay, show the pause menu again
        if (pauseMenu != null && PauseManager.GamePaused)
            pauseMenu.SetActive(true);
    }

    private void OnVolumeChanged(float value)
    {
        // Save for future
        PlayerPrefs.SetFloat("MasterVolume", value);

        // Later we plug this into real audio
        Debug.Log("Volume slider changed: " + value);
    }
}

