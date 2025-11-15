using UnityEngine;

public class PlayerFacing : Player
{
    private bool facingRight = true;

    public void UpdateFacingDirection()
    {
        if (cam == null) return;
        
        Vector2 mousePos = Input.mousePosition;
        Vector2 worldPos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, -cam.transform.position.z));
        
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
