using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image healthBarFill;
    [SerializeField] private Text healthText;

    private PlayerHealth playerHealth;
    private Vector3 originalScale;


    // Finds player health component and subscribes to changes.
    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogError("❌ No PlayerHealth found in scene!");
            return;
        }

        if (healthBarFill != null)
        {
            originalScale = healthBarFill.transform.localScale;
        }

        playerHealth.OnHealthChanged += UpdateHealthBar;
        UpdateHealthBar(playerHealth.CurrentHealth);

        Debug.Log("❤️ PlayerHealthBar initialized");
    }


    // Updates health bar image fill based on current health.
    private void UpdateHealthBar(float currentHealth)
    {
        if (playerHealth == null || healthBarFill == null) return;

        float healthPercent = playerHealth.HealthPercentage;

        // Update fill amount (for Filled Image type)
        healthBarFill.fillAmount = healthPercent;

        // Update text if exists
        if (healthText != null)
        {
            healthText.text = $"{Mathf.CeilToInt(currentHealth)} / {Mathf.CeilToInt(playerHealth.MaxHealth)}";
        }

        Debug.Log($"❤️ Health bar updated: {currentHealth}/{playerHealth.MaxHealth} ({healthPercent * 100:F0}%)");
    }


    // Unsubscribes from events.
    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthBar;
        }
    }
}