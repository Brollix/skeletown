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

<<<<<<< Updated upstream
        // Set health to full
=======
        StartCoroutine(RegisterWhenReady());

        ScaleStatsByLevel();

>>>>>>> Stashed changes
        health = maxHealth;

        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

<<<<<<< Updated upstream
    void Update() {
        if (player == null) {
=======
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
>>>>>>> Stashed changes
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
<<<<<<< Updated upstream
        if (distanceToPlayer > visionRadius) {
=======
        if (distanceToPlayer > visionRadius)
        {
            if (rb != null) rb.linearVelocity = Vector2.zero;
>>>>>>> Stashed changes
            return;
        }

        Vector2 moveDirection = (player.position - transform.position).normalized;
        Vector2 separation = CalculateSeparation();
        Vector2 finalDirection = (moveDirection + separation).normalized;

<<<<<<< Updated upstream
        finalDirection = finalDirection.normalized;

        rb.MovePosition(rb.position + finalDirection * speed * Time.deltaTime);
=======
        if (rb != null) rb.linearVelocity = finalDirection * speed;
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
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
=======
    public void TakeDamage(float amount)
>>>>>>> Stashed changes
    {
        health = Mathf.Max(0f, health - amount);
        OnHealthChanged?.Invoke(health);
        if (health <= 0f) Die();
    }

<<<<<<< Updated upstream
    Destroy(gameObject);
}


    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            // Damage player here later
=======
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
>>>>>>> Stashed changes
        }
    }
}
