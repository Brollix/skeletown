using UnityEngine;
using TMPro;

public class CheatUI : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private GameObject cheatPanel;

    [Header("Status Text (Optional)")]
    [SerializeField] private TextMeshProUGUI godModeStatus;
    [SerializeField] private TextMeshProUGUI speedStatus;

    private void Start()
    {
        if (cheatPanel != null)
            cheatPanel.SetActive(false);
            
        UpdateStatus(false, false);
    }

    public void TogglePanel()
    {
        if (cheatPanel != null)
            cheatPanel.SetActive(!cheatPanel.activeSelf);
    }

    public void UpdateStatus(bool isGodMode, bool isSpeedActive)
    {
        if (godModeStatus != null)
            godModeStatus.text = isGodMode ? GameConstants.UI_ON : GameConstants.UI_OFF;
            
        if (speedStatus != null)
            speedStatus.text = isSpeedActive ? GameConstants.UI_ON : GameConstants.UI_OFF;
    }
}
