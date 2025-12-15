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


    // Initializes health and notifies listeners.
    private void Start()
    {
        currentHealth = MaxHealth;
        OnHealthChanged?.Invoke(currentHealth);
    }


    // Applies damage to the player, triggering death or invincibility if needed.
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


    // Restores health up to the maximum limit.
    public void Heal(float amount)
    {
        if (IsDead) return;

        currentHealth = Mathf.Min(MaxHealth, currentHealth + amount);
        OnHealthChanged?.Invoke(currentHealth);
    }


    // Triggers death events and disables the player.
    private void Die()
    {
        if (IsDead) return;

        IsDead = true;
        OnDeath?.Invoke();
        OnPlayerDeath?.Invoke();
        // GameOverUI handles the game over screen
    }


    // Handles temporary invincibility duration.
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
