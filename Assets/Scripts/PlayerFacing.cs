using UnityEngine;

// Handles flipping the player sprite so it faces the mouse position.
public class PlayerFacing : MonoBehaviour
{
    private Player player;
    private bool facingRight = true;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    // Update facing direction based on mouse world position
    public void UpdateFacingDirection()
    {
        if (player == null || player.cam == null) return;
        
        Vector2 mousePos = Input.mousePosition;
        Vector2 worldPos = player.cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, -player.cam.transform.position.z));
        
        bool shouldFaceRight = worldPos.x > transform.position.x;
        
        if (shouldFaceRight != facingRight)
            Flip();
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (facingRight ? 1 : -1);
        transform.localScale = scale;
    }

    public bool IsFacingRight() => facingRight;
}
