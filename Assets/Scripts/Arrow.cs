using UnityEngine;

// Controls arrow movement, lifetime and collision with enemies.
public class Arrow : MonoBehaviour
{
    [Header("Arrow Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float rotationOffset = -90f;
    [SerializeField] private float lifetime = 5f;
    private float damage = 1f; // Set from PlayerShooting

    private Vector2 direction;
    private GameObject player;  // Add reference to player
    private Collider2D arrowCollider;

    // Initialize references and configure collisions
    private void Start()
    {
        // Get reference to the player and its colliders
        player = GameObject.FindGameObjectWithTag("Player");
        arrowCollider = GetComponent<Collider2D>();
        
        // Ignore collision with player if it exists
        if (player != null)
        {
            Collider2D[] playerColliders = player.GetComponentsInChildren<Collider2D>();
            foreach (Collider2D playerCollider in playerColliders)
            {
                if (playerCollider != null && arrowCollider != null)
                {
                    Physics2D.IgnoreCollision(arrowCollider, playerCollider);
                }
            }
        }
        
        // Auto-destroy in case it never hits anything
        Destroy(gameObject, lifetime);
    }

    public void setDirection(Vector2 dir)
    {
        direction = dir.normalized;

        // Rotate sprite to face movement direction
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);
    }

    // Set damage from the shooting player
    public void SetDamage(float value)
    {
        damage = value;
    }

    // Move the arrow forward every frame
    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Skip if this is a player-related object
        if (collision.CompareTag("Player") || 
            collision.GetComponent<PlayerMovement>() != null || 
            collision.GetComponent<BowController>() != null ||
            (player != null && collision.transform.IsChildOf(player.transform)))
        {
            return;
        }

        // Handle enemy collisions
        var enemy = collision.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            Debug.Log($"Arrow hit enemy! Dealing {damage} damage. Enemy health: {enemy.health}");
            enemy.TakeDamage(damage);
        }
        else
        {
            Debug.Log($"Arrow hit: {collision.gameObject.name}");
        }

        // Destroy the arrow upon any valid collision
        Destroy(gameObject);
    }
}
