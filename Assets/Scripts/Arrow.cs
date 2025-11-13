using UnityEngine;

public class Arrow : MonoBehaviour
{
    [Header("Arrow Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float rotationOffset = -90f;
    [SerializeField] private float lifetime = 5f;

    private Vector2 direction;

    private void Start()
    {
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

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignore collisions with the player or the bow itself
        if (collision.GetComponent<PlayerMovement>() != null) return;
        if (collision.GetComponent<BowController>() != null) return;

        // (Optional) Handle enemy collisions
        var enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            // Example: deal damage
            // enemy.TakeDamage(1);
        }

        // Destroy the arrow upon any valid collision
        Destroy(gameObject);
    }
}
