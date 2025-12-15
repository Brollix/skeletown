using UnityEngine;

// This script controls the arrow or projectile movement, lifetime and collision with enemies and walls.
public class Arrow : MonoBehaviour
{
    [Header("Arrow Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float rotationOffset = -90f;
    [SerializeField] private float lifetime = 5f;
    private float damage = 1f;

    private Vector2 direction;
    private GameObject player;
    private Collider2D arrowCollider;

    //This Start method initializes the reference to the player and configures the collision to make sure the arrow ignores any collision with the player if it happens. It then auto destroys the arrow object if it never hits anything to make sure it doesn't float around forever.
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        arrowCollider = GetComponent<Collider2D>();
        
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
        
        Destroy(gameObject, lifetime);
    }
    //This method ensures the arrow always heads out in the correct direction, forcing the sprite to face the direction in which the player was facing when they shot the arrow.
    public void setDirection(Vector2 dir)
    {
        direction = dir.normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);
    }

    //This method just sets the player damage.
    public void SetDamage(float value)
    {
        damage = value;
    }

    //This method here uses transform to change the arrow's direction, making it move forward across the screen in the direction it was shot in.
    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    public static event System.Action OnEnemyHit;

    //This method starts by checking if the arrow is colliding with the player or the bow to ensure it doesn't actually count as a collision. Once that's done, assuming the arrow has collided with an enemy, it checks if the enemy is still present and if they are, it makes the enemy take damage.
    //Finally, the arrow object is destroyed upon collision.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") ||
            collision.gameObject.GetComponent<PlayerMovement>() != null ||
            collision.gameObject.GetComponent<BowController>() != null ||
            (player != null && collision.gameObject.transform.IsChildOf(player.transform)))
        {
            return;
        }

        var enemy = collision.gameObject.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            OnEnemyHit?.Invoke();
        }

        Destroy(gameObject);
    }
}
