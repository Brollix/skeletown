using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Component references
    private Rigidbody2D _rb;
    private PlayerInput _input;
    private PlayerFacing _facing;
    private Camera _cam;
    private PlayerHealth _health;

    // Public properties with null checks and auto-get
    public Rigidbody2D rb => _rb != null ? _rb : _rb = GetComponent<Rigidbody2D>();
    public PlayerInput input => _input != null ? _input : _input = GetComponent<PlayerInput>();
    public PlayerFacing facing => _facing != null ? _facing : _facing = GetComponent<PlayerFacing>();
    public Camera cam => _cam != null ? _cam : _cam = Camera.main;
    public PlayerHealth health => _health != null ? _health : _health = GetComponent<PlayerHealth>();

    public static Player Instance { get; private set; }


    // Initializes the Singleton instance and identifies duplicate players.
    protected virtual void Awake()
    {
        if (Instance != null && Instance.gameObject != gameObject)
        {
            Debug.LogError($"[Player] Duplicate detected! Destroying new instance on {gameObject.name}. Existing instance on {Instance.gameObject.name} in scene {Instance.gameObject.scene.name}");
            Destroy(gameObject);
            return;
        }
        if (Instance == null)
        {
            Instance = this;
            Debug.Log($"[Player] Instance set on {gameObject.name} in scene {gameObject.scene.name} (Type: {this.GetType().Name})");
        }

        // Configure Rigidbody if it exists
        if (TryGetComponent<Rigidbody2D>(out var rbComponent))
        {
            _rb = rbComponent;
            rb.gravityScale = 0;
            rb.freezeRotation = true;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }
        else
        {
            Debug.LogError("No Rigidbody2D found on Player. Adding one...");
            _rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.freezeRotation = true;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        // Get other components if they exist
        _input = GetComponent<PlayerInput>();
        _facing = GetComponent<PlayerFacing>();
        _cam = Camera.main;
    }


    // Converts the screen mouse position to a world position.
    public Vector2 GetMousePosition()
    {
        if (cam == null) return Vector2.zero;
        Vector2 mousePos = Mouse.current?.position.ReadValue() ?? Vector2.zero;
        return cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, -cam.transform.position.z));
    }


    // Checks the visual facing direction of the player.
    public bool IsFacingRight() => facing != null && facing.IsFacingRight();
    

    // Applies velocity to the Rigidbody.
    public void Move(Vector2 direction, float speed)
    {
        if (rb == null) return;
        rb.linearVelocity = direction.normalized * speed;
    }


    // Halts player movement instantly.
    public void Stop()
    {
        if (rb != null) rb.linearVelocity = Vector2.zero;
    }
}
