using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFacing : MonoBehaviour
{
    private Player player;
    private bool facingRight = true;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    public void UpdateFacingDirection()
    {
        if (player == null || player.cam == null) return;

        Vector2 aimDirection = player.GetAimDirection();
        
        if (Mathf.Abs(aimDirection.x) > 0.1f)
        {
            bool shouldFaceRight = aimDirection.x > 0;
            if (shouldFaceRight != facingRight)
                Flip();
        }
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
