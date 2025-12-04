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

    public static PlayerHealth Instance { get; private set; }

    private void Awake()
    {
        // Singleton Logic with Scene Priority
        if (Instance != null && Instance != this)
        {
            // If the existing instance is in the MainMenu or Boot scene, and I am NOT,
            // then I am the "real" player for the game world. Destroy the old one.
            string oldScene = Instance.gameObject.scene.name;
            string myScene = gameObject.scene.name;

            if ((oldScene == "MainMenu" || oldScene == "Boot") && myScene != oldScene)
            {
                Debug.Log($"[PlayerHealth] Overwriting Singleton. Old: {oldScene}, New: {myScene}");
                Destroy(Instance.gameObject);
                Instance = this;
            }
            else
            {
                // Otherwise, I am the duplicate (or we are in the same scene)
                Destroy(gameObject);
                return;
            }
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        currentHealth = MaxHealth;
        OnHealthChanged?.Invoke(currentHealth);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    [Header("Audio")]
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;

    public void TakeDamage(float damage)
    {
        if (invincible || IsDead)
            return;

        currentHealth = Mathf.Max(0, currentHealth - damage);
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            if (AudioManager.Instance != null && hitSound != null)
            {
                AudioManager.Instance.PlaySFX(hitSound);
            }
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
        
        if (AudioManager.Instance != null && deathSound != null)
        {
            AudioManager.Instance.PlaySFX(deathSound);
        }
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
