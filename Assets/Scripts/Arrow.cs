using UnityEngine;

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

    public void setDirection(Vector2 dir)
    {
        direction = dir.normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);
    }

    public void SetDamage(float value)
    {
        damage = value;
    }

    private void Update()
    {
        if (PauseManager.GamePaused) return;

        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    public static event System.Action OnEnemyHit;

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
