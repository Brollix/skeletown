using UnityEngine;

public class PlayerMovement : Player
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    private void FixedUpdate()
    {
        if (input != null && input.moveInput != Vector2.zero)
        {
            Move(input.moveInput, moveSpeed);
        }
        else
        {
            Stop();
        }
        
        facing?.UpdateFacingDirection();
    }
}
