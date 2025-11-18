using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private float currentHealth;

    [Header("Invincibility")]
    [SerializeField] private float invincibilityDuration = 1f;
    [SerializeField] private bool invincible = false;

    public float MaxHealth => UpgradeManager.Instance?.MaxHealth ?? maxHealth;
    public float CurrentHealth => currentHealth;
    public float HealthPercentage => currentHealth / MaxHealth;
    public bool IsInvincible => invincible;

    public event Action<float> OnHealthChanged;
    public event Action OnDeath;

    private void Start()
    {
        currentHealth = MaxHealth;
        OnHealthChanged?.Invoke(currentHealth);
    }

    public void TakeDamage(float damage)
    {
        if (invincible)
        {
            // Debug.Log("🛡️ Player is invincible, damage ignored");
            return;
        }

        float oldHealth = currentHealth;
        currentHealth = Mathf.Max(0, currentHealth - damage);
        OnHealthChanged?.Invoke(currentHealth);

        // Debug.Log($"💥 Player took {damage} damage! Health: {oldHealth} → {currentHealth}");

        // Efecto visual de daño (opcional)
        // StartCoroutine(DamageFlash());

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Activar invencibilidad temporal
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    private System.Collections.IEnumerator DamageFlash()
    {
        // Flash rojo (si tienes sprite renderer)
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            Color originalColor = sprite.color;
            sprite.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sprite.color = originalColor;
        }
    }

    public void Heal(float amount)
    {
        float oldHealth = currentHealth;
        currentHealth = Mathf.Min(MaxHealth, currentHealth + amount);
        OnHealthChanged?.Invoke(currentHealth);
    }

    private void Die()
    {
        OnDeath?.Invoke();
        // Player defeated - game over screen will be shown by GameOverUI
    }

    private System.Collections.IEnumerator InvincibilityCoroutine()
    {
        invincible = true;
        // Aquí podrías añadir efectos visuales (parpadeo, etc.)
        float elapsed = 0f;
        while (elapsed < invincibilityDuration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        invincible = false;
    }
}