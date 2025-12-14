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
    public bool IsDead { get; private set; } = false; // safer encapsulation

    public event Action<float> OnHealthChanged;
    public event Action OnDeath;
    public static event Action OnPlayerDamage;
    public static event Action OnPlayerDeath; // Global static event for Audio

    private void Start()
    {
        currentHealth = MaxHealth;
        OnHealthChanged?.Invoke(currentHealth);
    }

    public void TakeDamage(float damage)
    {
        if (invincible || IsDead)
            return;

        currentHealth = Mathf.Max(0, currentHealth - damage);
        OnHealthChanged?.Invoke(currentHealth);
        OnPlayerDamage?.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    public void Heal(float amount)
    {
        if (IsDead) return;

        currentHealth = Mathf.Min(MaxHealth, currentHealth + amount);
        OnHealthChanged?.Invoke(currentHealth);
    }

    private void Die()
    {
        if (IsDead) return;

        IsDead = true;
        OnDeath?.Invoke();
        OnPlayerDeath?.Invoke();
        // GameOverUI handles the game over screen
    }

    private System.Collections.IEnumerator InvincibilityCoroutine()
    {
        invincible = true;
        float elapsed = 0f;
        while (elapsed < invincibilityDuration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        invincible = false;
    }
}
