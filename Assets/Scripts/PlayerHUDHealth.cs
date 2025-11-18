using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDHealth : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;
    [SerializeField] private Text healthText;

    private PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogError("❌ No PlayerHealth found for PlayerHUDHealth!");
            return;
        }

        playerHealth.OnHealthChanged += UpdateHealthDisplay;
        // Debug.Log("🎧 PlayerHUDHealth subscribed to OnHealthChanged event");
        UpdateHealthDisplay(playerHealth.CurrentHealth);

        // Debug.Log("❤️ PlayerHUDHealth initialized");
    }

    private void UpdateHealthDisplay(float currentHealth)
    {
        // Debug.Log($"🔄 UpdateHealthDisplay called with health: {currentHealth}");
        if (playerHealth == null) return;

        // Update health bar
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = playerHealth.HealthPercentage;
        }

        // Update health text
        if (healthText != null)
        {
            healthText.text = $"{Mathf.CeilToInt(currentHealth)}/{Mathf.CeilToInt(playerHealth.MaxHealth)}";
            // Debug.Log($"❤️ HUD Health updated: {currentHealth}/{playerHealth.MaxHealth}");
        }

        // Debug.Log($"❤️ HUD Health updated: {currentHealth}/{playerHealth.MaxHealth}");
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthDisplay;
        }
    }
}