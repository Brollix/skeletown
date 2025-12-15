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
            else
            {
                Debug.LogWarning("No PlayerHealth found for ExperienceHUD");
            }
        }
    }


    //Unsubscribes from events.
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


    //Handler for Level Up events.
    private void OnLevelUp(int newLevel)
    {
        UpdateUI();
    }


    //Handler for Experience Change events.
    private void OnXPChanged(float newXP)
    {
        UpdateUI();
    }


    //Handler for Health Change events.
    private void OnHealthChanged(float newHealth)
    {
        UpdateUI();
    }


    //Refreshes all HUD text and bars.
    private void UpdateUI()
    {
        if (PlayerExperience.Instance == null) return;

        if (levelText != null)
            levelText.text = $"Level {PlayerExperience.Instance.CurrentLevel}";

        if (xpText != null)
        {
            float currentXP = PlayerExperience.Instance.CurrentXP;
            float maxXP = PlayerExperience.Instance.XPForNextLevel;
            xpText.text = $"{Mathf.FloorToInt(currentXP)} / {Mathf.FloorToInt(maxXP)} XP";
        }

        if (xpBar != null)
        {
            float progress = PlayerExperience.Instance.CurrentXP / PlayerExperience.Instance.XPForNextLevel;
            xpBar.fillAmount = Mathf.Clamp01(progress);
        }

        if (skillPointsText != null)
            skillPointsText.text = $"Skill Points: {PlayerExperience.Instance.SkillPoints}";

        if (showHealthInHUD)
        {
            PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
            if (playerHealth != null)
            {
                if (healthText != null)
                {
                    healthText.text = $"{Mathf.CeilToInt(playerHealth.CurrentHealth)} / {Mathf.CeilToInt(playerHealth.MaxHealth)} HP";
                }

                if (healthBar != null)
                {
                    healthBar.fillAmount = playerHealth.HealthPercentage;
                }
            }
        }
    }
}