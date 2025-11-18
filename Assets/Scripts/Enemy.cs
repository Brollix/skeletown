using UnityEngine;
using System;

public class Enemy : MonoBehaviour {
    [Header("References")]
    [HideInInspector] public Transform player;

    [Header("Stats")]
    public float speed = 2f;
    public float health = 10f;
    public float maxHealth = 10f;
    public float damage = 2f;

    [Header("Flocking Settings")]
    public float separationRadius = 1f;
    public float separationForce = 2f;

    [Header("Vision Settings")]
    public float visionRadius = 7f;

    public int floorNumber;

    private Rigidbody2D rb;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();

        // Apply level-based scaling (based on player level, but no player upgrades)
        ScaleStatsByLevel();

        // Set health to full
        health = maxHealth;

        // Find player if missing
        if (player == null) {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null) {
                player = playerObj.transform;
            }
        }
    }

    private void ScaleStatsByLevel() {
        // For now, start enemies at level 1 - they will scale with player progress during gameplay
        // TODO: Add proper level progression system for enemies
        int enemyLevel = 1; // Always start at level 1 for new games

        // Scale enemy stats based on enemy level (not player level to avoid save file issues)
        float levelMultiplier = 1f + (enemyLevel - 1) * 0.2f; // 20% increase per level

        // Apply level scaling to base stats
        float originalSpeed = speed;
        speed *= levelMultiplier;
        maxHealth *= levelMultiplier;
        damage *= levelMultiplier;

        // Update current health to match max health
        health = maxHealth;

        Debug.Log($"🧟 Enemy stats scaled - Enemy Level: {enemyLevel}, Speed: {originalSpeed} → {speed}, Health: {maxHealth}, Damage: {damage}");
    }

    void Update() {
        if (player == null) {
            rb.linearVelocity = Vector2.zero;
            return;
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

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject other in enemies) {

            if (other == gameObject) {
                continue;
            }

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
    if (GameManager.Instance != null)
    {
        GameManager.Instance.EnemyDied(floorNumber);
    }

    // Give XP to player
    if (PlayerExperience.Instance != null)
    {
        float xpToGive = 10f; // Base XP, could be modified based on enemy type
        // Debug.Log($"🎯 Enemy died, giving {xpToGive} XP to player");
        PlayerExperience.Instance.AddXP(xpToGive);
    }

    Destroy(gameObject);
}


    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                if (!playerHealth.IsInvincible)
                {
                    playerHealth.TakeDamage(damage);
                }
            }
        }
    }
}
