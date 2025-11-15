using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : Player
{
    public Vector2 moveInput { get; private set; }
    private PlayerControls controls;
    private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        controls = new PlayerControls();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Move.performed += OnMove;
        controls.Player.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        controls.Player.Move.performed -= OnMove;
        controls.Player.Move.canceled -= OnMove;
        controls.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        animator?.SetBool("isMoving", moveInput != Vector2.zero);
    }
}
