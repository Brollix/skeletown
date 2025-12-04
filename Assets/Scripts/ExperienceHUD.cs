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
        StartCoroutine(InitializeHUD());
    }

    private System.Collections.IEnumerator InitializeHUD()
    {
        // Wait a frame to ensure singletons are ready
        yield return null;

        UpdateUI();

        if (PlayerExperience.Instance != null)
        {
            PlayerExperience.Instance.OnLevelUp += OnLevelUp;
            PlayerExperience.Instance.OnXPChanged += OnXPChanged;
        }

        // Wait for PlayerHealth singleton
        if (showHealthInHUD)
        {
            while (PlayerHealth.Instance == null)
            {
                yield return null;
            }

            PlayerHealth playerHealth = PlayerHealth.Instance;
            playerHealth.OnHealthChanged += OnHealthChanged;
            // Force an update now that we have the player
            OnHealthChanged(playerHealth.CurrentHealth);
            Debug.Log("🎧 ExperienceHUD subscribed to player health changes via Singleton");
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

        // Update level
        if (levelText != null)
            levelText.text = $"Level {PlayerExperience.Instance.CurrentLevel}";

        // Update XP
        if (xpText != null)
        {
            float currentXP = PlayerExperience.Instance.CurrentXP;
            float maxXP = PlayerExperience.Instance.XPForNextLevel;
            xpText.text = $"{Mathf.FloorToInt(currentXP)} / {Mathf.FloorToInt(maxXP)} XP";
        }

        // Update XP bar
        if (xpBar != null)
        {
            float progress = PlayerExperience.Instance.CurrentXP / PlayerExperience.Instance.XPForNextLevel;
            xpBar.fillAmount = Mathf.Clamp01(progress);
        }

        // Update skill points
        if (skillPointsText != null)
            skillPointsText.text = $"Skill Points: {PlayerExperience.Instance.SkillPoints}";

        // Update health (only if enabled and elements exist)
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