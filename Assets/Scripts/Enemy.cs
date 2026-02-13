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

        ScaleStatsByLevel();

        health = maxHealth;

        if (Player.Instance != null) {
            player = Player.Instance.transform;
        }
    }

    private void ScaleStatsByLevel() {
        int enemyLevel = floorNumber;

        float levelMultiplier = 1f + (enemyLevel - 1) * 0.2f;

        speed *= levelMultiplier;
        maxHealth *= levelMultiplier;
        damage *= levelMultiplier;

        health = maxHealth;
    }

    void Update() {
        if (PauseManager.GamePaused) {
            rb.linearVelocity = Vector2.zero;
            return;
        }

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

        if (PlayerExperience.Instance != null)
        {
            float xpToGive = 60f;
            PlayerExperience.Instance.AddXP(xpToGive);
        }

        if (isBoss)
        {
            PlayerHealth ph = FindObjectOfType<PlayerHealth>();

            if (ph != null)
            {
                if (!ph.IsDead)
                {
                    VictoryUI ui = FindObjectOfType<VictoryUI>(true);
                    if (ui != null)
                        ui.ShowVictory();
                }
            }
        }

        Destroy(gameObject);
    }

    private void HandlePlayerCollision(GameObject other)
    {
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
