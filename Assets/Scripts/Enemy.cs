using UnityEngine;
using System;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [HideInInspector] public Transform player;

    [Header("Stats")]
    public float speed = 1f;
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

    public event Action<float> OnHealthChanged;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(RegisterWhenReady());

        ScaleStatsByLevel();

        health = maxHealth;

        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    private IEnumerator RegisterWhenReady()
    {
        while (GameManager.Instance == null)
            yield return null;

        GameManager.Instance.RegisterEnemy(floorNumber);
    }

    private void ScaleStatsByLevel()
    {
        int enemyLevel = floorNumber;
        float levelMultiplier = 1f + (enemyLevel - 1) * 0.2f;

        float originalSpeed = speed;
        speed *= levelMultiplier;
        maxHealth *= levelMultiplier;
        damage *= levelMultiplier;

        health = maxHealth;

        //Debug.Log($"Enemy stats scaled - Enemy Level: {enemyLevel}, Speed: {originalSpeed} → {speed}, Health: {maxHealth}, Damage: {damage}");
    }

    private void Update()
    {
        if (player == null)
        {
            if (rb != null) rb.linearVelocity = Vector2.zero;
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer > visionRadius)
        {
            if (rb != null) rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 moveDirection = (player.position - transform.position).normalized;
        Vector2 separation = CalculateSeparation();
        Vector2 finalDirection = (moveDirection + separation).normalized;

        if (rb != null) rb.linearVelocity = finalDirection * speed;
    }

    private Vector2 CalculateSeparation()
    {
        Vector2 separationMove = Vector2.zero;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject other in enemies)
        {
            if (other == gameObject) continue;

            float distance = Vector2.Distance(transform.position, other.transform.position);
            if (distance < separationRadius)
            {
                Vector2 pushDirection = (transform.position - other.transform.position).normalized;
                separationMove += pushDirection / distance;
            }
        }

        return separationMove * separationForce;
    }

    public void TakeDamage(float amount)
    {
        health = Mathf.Max(0f, health - amount);
        OnHealthChanged?.Invoke(health);
        if (health <= 0f) Die();
    }

    private void Die()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.EnemyKilled(floorNumber);

        if (PlayerExperience.Instance != null)
            PlayerExperience.Instance.AddXP(50f);

        Destroy(gameObject);
        Debug.Log($"[DEBUG] Enemy.Die called on floor {floorNumber} for {gameObject.name}", this);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null && !playerHealth.IsInvincible)
                playerHealth.TakeDamage(damage);
        }
    }
}
