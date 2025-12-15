using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFacing : MonoBehaviour
{
    private Player player;
    private bool facingRight = true;


    // Gets reference to the main player script.
    private void Awake()
    {
        player = GetComponent<Player>();
    }


    // Calculates mouse position and flips the character if needed.
    public void UpdateFacingDirection()
    {
        if (player == null || player.cam == null) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 worldPos = player.cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, -player.cam.transform.position.z));
        
        bool shouldFaceRight = worldPos.x > transform.position.x;
        
        if (shouldFaceRight != facingRight)
            Flip();
    }


    // Inverts the X scale of the transform.
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (facingRight ? 1 : -1);
        transform.localScale = scale;
    }


    // Returns true if the character is looking to the right.
    public bool IsFacingRight() => facingRight;
}
