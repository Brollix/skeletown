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

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    public void UpgradeHealth()
    {
        if (UpgradeManager.Instance.UpgradeHealth())
            UpdateUI();
    }

    public void UpgradeDamage()
    {
        if (UpgradeManager.Instance.UpgradeDamage())
            UpdateUI();
    }

    public void UpgradeSpeed()
    {
        if (UpgradeManager.Instance.UpgradeSpeed())
            UpdateUI();
    }

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

    private void UpdateUI()
    {
        skillPointsText.text = "Skill Points: " + PlayerExperience.Instance.SkillPoints;

        healthLevelValue.text = UpgradeManager.Instance.GetHealthUpgrades().ToString();
        damageLevelValue.text = UpgradeManager.Instance.GetDamageUpgrades().ToString();
        speedLevelValue.text = UpgradeManager.Instance.GetSpeedUpgrades().ToString();

        bool hasPoints = PlayerExperience.Instance.SkillPoints > 0;

        healthButton.interactable = hasPoints;
        damageButton.interactable = hasPoints;
        speedButton.interactable = hasPoints;

        bool hasUpgrades =
            (UpgradeManager.Instance.GetHealthUpgrades()
            + UpgradeManager.Instance.GetDamageUpgrades()
            + UpgradeManager.Instance.GetSpeedUpgrades()) > 0;

        resetButton.interactable = hasUpgrades;
    }
}
