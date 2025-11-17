using UnityEngine;

// Scales a child bar based on the associated enemy's current health.
public class HealthBar : MonoBehaviour
{
    private Enemy enemy;
    private Vector3 originalScale;

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        if (enemy == null) return;
        
        originalScale = transform.localScale;
        enemy.OnHealthChanged += UpdateHealth;
        UpdateHealth(enemy.health);
    }

    // Adjust bar scale based on health percentage
    private void UpdateHealth(float currentHealth)
    {
        float healthPercent = Mathf.Clamp01(currentHealth / enemy.maxHealth);
        transform.localScale = new Vector3(
            originalScale.x * healthPercent,
            originalScale.y,
            originalScale.z
        );
    }

    private void OnDestroy()
    {
        if (enemy != null)
            enemy.OnHealthChanged -= UpdateHealth;
    }
}