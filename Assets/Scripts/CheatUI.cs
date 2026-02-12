using UnityEngine;
using TMPro;

public class CheatUI : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private GameObject cheatPanel;

    [Header("Status Text (Optional)")]
    // Assign these if you want dynamic "ON/OFF" text updates.
    [SerializeField] private TextMeshProUGUI godModeStatus;
    [SerializeField] private TextMeshProUGUI speedStatus;

    private void Start()
    {
        // Ensure panel is hidden on start
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
            godModeStatus.text = isGodMode ? "<color=green>[ON]</color>" : "<color=red>[OFF]</color>";
            
        if (speedStatus != null)
            speedStatus.text = isSpeedActive ? "<color=green>[ON]</color>" : "<color=red>[OFF]</color>";
    }
}
