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
            return;
        }

        playerHealth.OnHealthChanged += UpdateHealthDisplay;
        UpdateHealthDisplay(playerHealth.CurrentHealth);
    }

    private void UpdateHealthDisplay(float currentHealth)
    {
        if (playerHealth == null) return;

        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = playerHealth.HealthPercentage;
        }

        if (healthText != null)
        {
            healthText.text = $"{Mathf.CeilToInt(currentHealth)}/{Mathf.CeilToInt(playerHealth.MaxHealth)}";
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthDisplay;
        }
    }
}