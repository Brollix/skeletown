using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rb;
    private PlayerInput _input;
    private PlayerFacing _facing;
    private Camera _cam;
    private PlayerHealth _health;

    public Rigidbody2D rb => _rb != null ? _rb : _rb = GetComponent<Rigidbody2D>();
    public PlayerInput input => _input != null ? _input : _input = GetComponent<PlayerInput>();
    public PlayerFacing facing => _facing != null ? _facing : _facing = GetComponent<PlayerFacing>();
    public Camera cam => _cam != null ? _cam : _cam = Camera.main;
    public PlayerHealth health => _health != null ? _health : _health = GetComponent<PlayerHealth>();

    public static Player Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null && Instance.gameObject != gameObject)
        {
            Destroy(gameObject);
            return;
        }
        if (Instance == null)
        {
            Instance = this;
        }

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
            _rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.freezeRotation = true;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        _input = GetComponent<PlayerInput>();
        _facing = GetComponent<PlayerFacing>();
        _cam = Camera.main;
    }

    public Vector2 GetMousePosition()
    {
        if (cam == null) return Vector2.zero;
        Vector2 mousePos = Mouse.current?.position.ReadValue() ?? Vector2.zero;
        return cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, -cam.transform.position.z));
    }

    public Vector2 GetAimDirection()
    {
        if (input != null)
        {
            return input.CurrentAimDirection;
        }

        Vector2 mousePos = GetMousePosition();
        return (mousePos - (Vector2)transform.position).normalized;
    }

    public bool IsFacingRight() => facing != null && facing.IsFacingRight();
    
    public void Move(Vector2 direction, float speed)
    {
        if (rb == null) return;
        rb.linearVelocity = direction.normalized * speed;
    }

    public void Stop()
    {
        if (rb != null) rb.linearVelocity = Vector2.zero;
    }
}
