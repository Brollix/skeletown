using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperienceHUD : MonoBehaviour
{
    [Header("Experience UI Elements")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private Image xpBar;
    [SerializeField] private TextMeshProUGUI skillPointsText;

    [Header("Health UI Elements")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image healthBar;

    [Header("Settings")]
    [SerializeField] private bool showHealthInHUD = true;

    private void Start()
    {
        UpdateUI();

        if (PlayerExperience.Instance != null)
        {
            PlayerExperience.Instance.OnLevelUp += OnLevelUp;
            PlayerExperience.Instance.OnXPChanged += OnXPChanged;
        }

        if (showHealthInHUD)
        {
            PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.OnHealthChanged += OnHealthChanged;
            }
        }
    }

    private void OnDestroy()
    {
        if (PlayerExperience.Instance != null)
        {
            PlayerExperience.Instance.OnLevelUp -= OnLevelUp;
            PlayerExperience.Instance.OnXPChanged -= OnXPChanged;
        }

        if (showHealthInHUD)
        {
            PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.OnHealthChanged -= OnHealthChanged;
            }
        }
    }

    private void OnLevelUp(int newLevel)
    {
        UpdateUI();
    }

    private void OnXPChanged(float newXP)
    {
        UpdateUI();
    }

    private void OnHealthChanged(float newHealth)
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (PlayerExperience.Instance == null) return;

        if (levelText != null)
            levelText.text = $"{GameConstants.UI_LEVEL_PREFIX}{PlayerExperience.Instance.CurrentLevel}";

        if (xpText != null)
        {
            float currentXP = PlayerExperience.Instance.CurrentXP;
            float maxXP = PlayerExperience.Instance.XPForNextLevel;
            xpText.text = $"{Mathf.FloorToInt(currentXP)}{GameConstants.UI_SLASH_SPACED}{Mathf.FloorToInt(maxXP)}{GameConstants.UI_XP_SUFFIX}";
        }

        if (xpBar != null)
        {
            float progress = PlayerExperience.Instance.CurrentXP / PlayerExperience.Instance.XPForNextLevel;
            xpBar.fillAmount = Mathf.Clamp01(progress);
        }

        if (skillPointsText != null)
            skillPointsText.text = $"{GameConstants.UI_SKILL_POINTS_PREFIX}{PlayerExperience.Instance.SkillPoints}";

        if (showHealthInHUD)
        {
            PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
            if (playerHealth != null)
            {
                if (healthText != null)
                {
                    healthText.text = $"{Mathf.CeilToInt(playerHealth.CurrentHealth)}{GameConstants.UI_SLASH_SPACED}{Mathf.CeilToInt(playerHealth.MaxHealth)}{GameConstants.UI_HP_SUFFIX}";
                }

                if (healthBar != null)
                {
                    healthBar.fillAmount = playerHealth.HealthPercentage;
                }
            }
        }
    }
}