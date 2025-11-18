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
    [SerializeField] private TextMeshProUGUI healthLevelLabel;
    [SerializeField] private TextMeshProUGUI healthLevelValue;
    [SerializeField] private TextMeshProUGUI damageLevelLabel;
    [SerializeField] private TextMeshProUGUI damageLevelValue;
    [SerializeField] private TextMeshProUGUI speedLevelLabel;
    [SerializeField] private TextMeshProUGUI speedLevelValue;

    private void Start()
    {
        InitializeLabels();
        UpdateUI();
    }

    private void InitializeLabels()
    {
        // Set static label texts
        if (healthLevelLabel != null) healthLevelLabel.text = "Health Level:";
        if (damageLevelLabel != null) damageLevelLabel.text = "Damage Level:";
        if (speedLevelLabel != null) speedLevelLabel.text = "Speed Level:";
    }

    private void Update()
    {
        UpdateUI();
    }

    public void UpgradeHealth()
    {
        if (UpgradeManager.Instance != null && PlayerExperience.Instance != null)
        {
            if (PlayerExperience.Instance.SkillPoints > 0)
            {
                if (UpgradeManager.Instance.UpgradeHealth())
                {
                    UpdateUI();
                }
            }
        }
    }

    public void UpgradeDamage()
    {
        if (UpgradeManager.Instance != null && PlayerExperience.Instance != null)
        {
            if (PlayerExperience.Instance.SkillPoints > 0)
            {
                if (UpgradeManager.Instance.UpgradeDamage())
                {
                    UpdateUI();
                }
            }
        }
    }

    public void UpgradeSpeed()
    {
        if (UpgradeManager.Instance != null && PlayerExperience.Instance != null)
        {
            if (PlayerExperience.Instance.SkillPoints > 0)
            {
                if (UpgradeManager.Instance.UpgradeSpeed())
                {
                    UpdateUI();
                }
            }
        }
    }

    public void ResetUpgrades()
    {
        Debug.Log("🔄 ResetUpgrades() called!");
        if (UpgradeManager.Instance != null && PlayerExperience.Instance != null)
        {
            // Calculate total skill points used
            int healthUpgrades = UpgradeManager.Instance.GetHealthUpgrades();
            int damageUpgrades = UpgradeManager.Instance.GetDamageUpgrades();
            int speedUpgrades = UpgradeManager.Instance.GetSpeedUpgrades();

            int totalPointsUsed = healthUpgrades + damageUpgrades + speedUpgrades;
            Debug.Log($"🔄 Resetting upgrades - Health: {healthUpgrades}, Damage: {damageUpgrades}, Speed: {speedUpgrades}, Total points: {totalPointsUsed}");

            // Reset all upgrades to 0
            UpgradeManager.Instance.ResetAllUpgrades();

            // Return skill points to player
            PlayerExperience.Instance.AddSkillPoints(totalPointsUsed);

            // Save changes
            UpgradeManager.Instance.SaveUpgrades();
            PlayerExperience.Instance.SaveProgress();

            // Update UI
            UpdateUI();
            Debug.Log("✅ Reset completed successfully!");
        }
        else
        {
            Debug.LogError("❌ Cannot reset: Managers not found");
        }
    }

    private void UpdateUI()
    {
        if (PlayerExperience.Instance == null)
        {
            Debug.LogError("PlayerExperience.Instance is null!");
            return;
        }
        if (UpgradeManager.Instance == null)
        {
            Debug.LogError("UpgradeManager.Instance is null!");
            return;
        }

        // Update skill points
        if (skillPointsText != null)
        {
            skillPointsText.text = $"Skill Points: {PlayerExperience.Instance.SkillPoints}";
        }
        else
        {
            Debug.LogWarning("⚠️ skillPointsText is null!");
        }

        // Update upgrade level values
        if (healthLevelValue != null)
            healthLevelValue.text = UpgradeManager.Instance.GetHealthUpgrades().ToString();
        if (damageLevelValue != null)
            damageLevelValue.text = UpgradeManager.Instance.GetDamageUpgrades().ToString();
        if (speedLevelValue != null)
            speedLevelValue.text = UpgradeManager.Instance.GetSpeedUpgrades().ToString();

        // Enable/disable buttons based on available skill points
        bool hasSkillPoints = PlayerExperience.Instance.SkillPoints > 0;
        if (healthButton != null) healthButton.interactable = hasSkillPoints;
        if (damageButton != null) damageButton.interactable = hasSkillPoints;
        if (speedButton != null) speedButton.interactable = hasSkillPoints;

        // Enable reset button if there are any upgrades to reset
        bool hasUpgrades = (UpgradeManager.Instance.GetHealthUpgrades() +
                           UpgradeManager.Instance.GetDamageUpgrades() +
                           UpgradeManager.Instance.GetSpeedUpgrades()) > 0;
        if (resetButton != null) resetButton.interactable = hasUpgrades;
    }
}