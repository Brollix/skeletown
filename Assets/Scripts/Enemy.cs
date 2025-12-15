using UnityEngine;
using System;

public class Enemy : MonoBehaviour {
    [Header("References")]
    [HideInInspector] public Transform player;

    [Header("Stats")]
    public float speed = 1f;
    public float health = 2f;
    public float maxHealth = 2f;
    public float damage = 1f;

    [Header("Flocking Settings")]
    public float separationRadius = 1f;
    public float separationForce = 2f;

    [Header("Vision Settings")]
    public float visionRadius = 7f;

    public int floorNumber;

    public bool isBoss = false;

    private Rigidbody2D rb;

    // Optimization: Cache enemies list to avoid FindGameObjectsWithTag in Update
    public static System.Collections.Generic.List<Enemy> ActiveEnemies = new System.Collections.Generic.List<Enemy>();

    private void OnEnable()
    {
        ActiveEnemies.Add(this);
    }

    private void OnDisable()
    {
        ActiveEnemies.Remove(this);
    }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();

        // Debug log to track duplicate spawning
        Debug.Log($"Enemy spawned at {transform.position} (Floor {floorNumber})");

        // Apply level-based scaling (based on player level, but no player upgrades)
        ScaleStatsByLevel();

        // Set health to full
        health = maxHealth;

        // Try to finding player via Singleton
        if (Player.Instance != null) {
            player = Player.Instance.transform;
        }
    }

    private void ScaleStatsByLevel() {
        // For now, start enemies at level 1 - they will scale with player progress during gameplay
        // TODO: Add proper level progression system for enemies
        int enemyLevel = floorNumber; // Always start at level 1 for new games

        // Scale enemy stats based on enemy level (not player level to avoid save file issues)
        float levelMultiplier = 1f + (enemyLevel - 1) * 0.2f; // 20% increase per level

        // Apply level scaling to base stats
        float originalSpeed = speed;
        speed *= levelMultiplier;
        maxHealth *= levelMultiplier;
        damage *= levelMultiplier;

        // Update current health to match max health
        health = maxHealth;

        //Debug.Log($"Enemy stats scaled - Enemy Level: {enemyLevel}, Speed: {originalSpeed} → {speed}, Health: {maxHealth}, Damage: {damage}");
    }

    void Update() {
        // Retry finding player if null (handles race condition where Enemy spawns before Player)
        if (player == null) {
            if (Player.Instance != null) {
                player = Player.Instance.transform;
            } else {
                rb.linearVelocity = Vector2.zero;
                return;
            }
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer > visionRadius) {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 moveDirection = (player.position - transform.position).normalized;

        Vector2 separation = CalculateSeparation();
        Vector2 finalDirection = moveDirection + separation;

        finalDirection = finalDirection.normalized;

        rb.linearVelocity = finalDirection * speed;
    }

    Vector2 CalculateSeparation() {
        Vector2 separationMove = Vector2.zero;

        // Use cached list instead of FindGameObjectsWithTag
        foreach (Enemy other in ActiveEnemies) {
            if (other == this) continue;

            float distance = Vector2.Distance(transform.position, other.transform.position);

            if (distance < separationRadius) {
                Vector2 pushDirection = (transform.position - other.transform.position).normalized;

                separationMove += pushDirection / distance;
            }
        }

        return separationMove * separationForce;
    }

    public void TakeDamage(float amount) {
        float oldHealth = health;
        float newHealth = health - amount;

        if (newHealth < 0f) {
            newHealth = 0f;
        }

        health = newHealth;

        if (OnHealthChanged != null) {
            OnHealthChanged(health);
        }

        if (health <= 0f) {
            Die();
        }
    }

    public event Action<float> OnHealthChanged;

    void Die()
    {
        // Notify game manager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EnemyDied(floorNumber);
        }

        // Give XP to player
        if (PlayerExperience.Instance != null)
        {
            float xpToGive = 60f;
            PlayerExperience.Instance.AddXP(xpToGive);
        }

        // === SAFETY: only trigger victory if this is the boss AND the player is still alive ===
        if (isBoss)
        {
            // try PlayerHealth singleton/reference first if you have one
            PlayerHealth ph = FindObjectOfType<PlayerHealth>();

            // If there's no Player object, don't trigger victory
            if (ph == null)
            {
                Debug.LogWarning("Enemy.Die: PlayerHealth not found; skipping victory trigger.");
            }
            else
            {
                // Only show victory if player is NOT dead
                if (!ph.IsDead)
                {
                    VictoryUI ui = FindObjectOfType<VictoryUI>(true);
                    if (ui != null)
                        ui.ShowVictory();
                    else
                        Debug.LogWarning("IsBoss defeated, but no VictoryUI found in scene!");
                }
                else
                {
                    // Player already dead → do not show victory
                    Debug.Log("Enemy.Die: player already dead — skipping victory.");
                }
            }
        }

        Destroy(gameObject);
    }



    private void HandlePlayerCollision(GameObject other)
    {
        // Removed strict Tag check to allow damage on any object with PlayerHealth
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        
        if (playerHealth != null)
        {
            if (!playerHealth.IsInvincible)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        HandlePlayerCollision(collision.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        HandlePlayerCollision(collider.gameObject);
    }
}
