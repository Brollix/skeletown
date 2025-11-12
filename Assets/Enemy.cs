using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [HideInInspector] public Transform player;

    [Header("Stats")]
    public float speed = 2f;
    public float health = 10f;
    public float damage = 2f;

    [Header("Flocking Settings")]
    public float separationRadius = 1f;   // Minimum distance between enemies
    public float separationForce = 2f;    // Strength of separation

    [Header("Vision Settings")]
    public bool isVisible = false; // Is the enemy currently visible on screen

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // If player reference is not set, try to find it by tag
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    void Update()
    {
        // Do not move if there is no player or enemy is off-screen
        if (player == null || !isVisible)
            return;

        Vector2 moveDirection = (player.position - transform.position).normalized;

        // Apply separation to avoid crowding
        Vector2 separation = CalculateSeparation();
        Vector2 finalDirection = (moveDirection + separation).normalized;

        rb.MovePosition(rb.position + finalDirection * speed * Time.deltaTime);
    }

    // Called when object becomes visible by any camera
    void OnBecameVisible() => isVisible = true;

    // Called when object goes off-screen
    void OnBecameInvisible() => isVisible = false;

    // Calculate separation vector to prevent enemies from stacking
    Vector2 CalculateSeparation()
    {
        Vector2 separationMove = Vector2.zero;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject other in enemies)
        {
            if (other == gameObject) continue;
            float distance = Vector2.Distance(transform.position, other.transform.position);
            if (distance < separationRadius)
            {
                Vector2 pushDir = (transform.position - other.transform.position).normalized;
                separationMove += pushDir / distance; // closer means stronger push
            }
        }

        return separationMove * separationForce;
    }

    // Public method to receive damage
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    // Destroy enemy when health reaches zero
    void Die()
    {
        // You can add particle effects, sounds, or animations here
        Destroy(gameObject);
    }

    // Deal damage to player on collision (to implement later)
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Example: call a method on the player to reduce health
            // collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }
}
