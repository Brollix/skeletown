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

    void Update() {
        if (player == null) {
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer > visionRadius) {
            return;
        }

        Vector2 moveDirection = (player.position - transform.position).normalized;

        Vector2 separation = CalculateSeparation();
        Vector2 finalDirection = moveDirection + separation;

        finalDirection = finalDirection.normalized;

        rb.MovePosition(rb.position + finalDirection * speed * Time.deltaTime);
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
