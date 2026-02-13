using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image healthBarFill;
    [SerializeField] private Text healthText;

    private PlayerHealth playerHealth;
    private Vector3 originalScale;

    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth == null)
        {
            return;
        }

        if (healthBarFill != null)
        {
            originalScale = healthBarFill.transform.localScale;
        }

        playerHealth.OnHealthChanged += UpdateHealthBar;
        UpdateHealthBar(playerHealth.CurrentHealth);
    }

    private void UpdateHealthBar(float currentHealth)
    {
        if (playerHealth == null || healthBarFill == null) return;

        float healthPercent = playerHealth.HealthPercentage;

        healthBarFill.fillAmount = healthPercent;

        if (healthText != null)
        {
            healthText.text = $"{Mathf.CeilToInt(currentHealth)} / {Mathf.CeilToInt(playerHealth.MaxHealth)}";
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthBar;
        }
    }
}