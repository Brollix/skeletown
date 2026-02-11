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

        if (player == null || player.cam == null) return;
        
        Vector2 aimDirection = player.GetAimDirection();
        
        // Face right if aim X is positive, Left if negative
        // If aim is perfectly vertical (x=0), keep current facing
        if (Mathf.Abs(aimDirection.x) > 0.1f)
        {
            bool shouldFaceRight = aimDirection.x > 0;
            if (shouldFaceRight != facingRight)
                Flip();
        }
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
