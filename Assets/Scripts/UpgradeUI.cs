using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI skillPointsText;
    [SerializeField] private Button healthButton;
    [SerializeField] private Button damageButton;
    [SerializeField] private Button speedButton;
    [SerializeField] private Button resetButton;

    [Header("Level Display")]
    [SerializeField] private TextMeshProUGUI healthLevelValue;
    [SerializeField] private TextMeshProUGUI damageLevelValue;
    [SerializeField] private TextMeshProUGUI speedLevelValue;


    // Initializes the UI display.
    private void Start()
    {
        UpdateUI();
    }


    // Continually refreshes valid button states.
    private void Update()
    {
        UpdateUI();
    }


    // Attempts to upgrade health via manager.
    public void UpgradeHealth()
    {
        if (UpgradeManager.Instance.UpgradeHealth())
            UpdateUI();
    }


    // Attempts to upgrade damage via manager.
    public void UpgradeDamage()
    {
        if (UpgradeManager.Instance.UpgradeDamage())
            UpdateUI();
    }


    // Attempts to upgrade speed via manager.
    public void UpgradeSpeed()
    {
        if (UpgradeManager.Instance.UpgradeSpeed())
            UpdateUI();
    }


    // Refunds all spent points and resets upgrades.
    public void ResetUpgrades()
    {
        int total = UpgradeManager.Instance.GetHealthUpgrades()
                 + UpgradeManager.Instance.GetDamageUpgrades()
                 + UpgradeManager.Instance.GetSpeedUpgrades();

        UpgradeManager.Instance.ResetAllUpgrades();
        PlayerExperience.Instance.AddSkillPoints(total);

        UpgradeManager.Instance.SaveUpgrades();
        PlayerExperience.Instance.SaveProgress();

        UpdateUI();
    }


    // Updates text fields and button interactivity.
    private void UpdateUI()
    {
        skillPointsText.text = "Skill Points: " + PlayerExperience.Instance.SkillPoints;

        healthLevelValue.text = UpgradeManager.Instance.GetHealthUpgrades().ToString();
        damageLevelValue.text = UpgradeManager.Instance.GetDamageUpgrades().ToString();
        speedLevelValue.text = UpgradeManager.Instance.GetSpeedUpgrades().ToString();

        bool hasPoints = PlayerExperience.Instance.SkillPoints > 0;

        healthButton.interactable = hasPoints && !UpgradeManager.Instance.IsHealthMaxed();
        damageButton.interactable = hasPoints && !UpgradeManager.Instance.IsDamageMaxed();
        speedButton.interactable = hasPoints && !UpgradeManager.Instance.IsSpeedMaxed();

        bool hasUpgrades =
            (UpgradeManager.Instance.GetHealthUpgrades()
            + UpgradeManager.Instance.GetDamageUpgrades()
            + UpgradeManager.Instance.GetSpeedUpgrades()) > 0;

        resetButton.interactable = hasUpgrades;
    }
}
