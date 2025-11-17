using UnityEngine;
using System;

// Basic enemy that moves towards the player with simple flocking and health.
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
    public bool isVisible = false;

    public int floorNumber;

    private Rigidbody2D rb;

    // Initialize components and find the player if needed
    private void Start() {
        rb = GetComponent<Rigidbody2D>();

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

    // Move towards the player while applying separation from other enemies
    void Update() {
        if (player == null) {
            return;
        }

        if (!isVisible) {
            return;
        }

        Vector2 moveDirection = (player.position - transform.position).normalized;

        Vector2 separation = CalculateSeparation();
        Vector2 finalDirection = moveDirection + separation;

        finalDirection = finalDirection.normalized;

        rb.MovePosition(rb.position + finalDirection * speed * Time.deltaTime);
    }

    void OnBecameVisible() {
        isVisible = true;
    }

    void OnBecameInvisible() {
        isVisible = false;
    }

    // Calculate a separation vector from nearby enemies to avoid clustering
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

    // Apply damage, invoke health changed event and check for death
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

    Destroy(gameObject);
}


    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            // Damage player here later
        }
    }
}
