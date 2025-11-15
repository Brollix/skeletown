using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Todo público para acceso directo
    public Rigidbody2D rb;
    public PlayerInput input;
    public PlayerFacing facing;
    public Camera cam;

    protected virtual void Awake()
    {
        // Obtener componentes si no están asignados
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (input == null) input = GetComponent<PlayerInput>();
        if (facing == null) facing = GetComponent<PlayerFacing>();
        if (cam == null) cam = Camera.main;
        
        // Configuración básica del Rigidbody
        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.freezeRotation = true;
        }
    }

    // Métodos de utilidad
    public Vector2 GetMousePosition()
    {
        if (cam == null) return Vector2.zero;
        Vector2 mousePos = Mouse.current?.position.ReadValue() ?? Vector2.zero;
        return cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, -cam.transform.position.z));
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
