using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : Player
{
    public Vector2 moveInput { get; private set; }
    private PlayerControls controls;
    private Animator _animator;
    private Animator animator => _animator != null ? _animator : _animator = GetComponent<Animator>();
    private BowController _bowController;
    private Transform bowTransform => _bowController != null ? _bowController.transform : (_bowController = GetComponentInChildren<BowController>())?.transform;

    protected override void Awake()
    {
        base.Awake();
        controls = new PlayerControls();
        _animator = GetComponent<Animator>();
        _bowController = GetComponentInChildren<BowController>();
    }

    private void OnEnable()
    {
        if (controls == null) controls = new PlayerControls();
        controls.Player.Enable();
        controls.Player.Move.performed += OnMove;
        controls.Player.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        if (controls != null)
        {
            controls.Player.Move.performed -= OnMove;
            controls.Player.Move.canceled -= OnMove;
            controls.Player.Disable();
        }
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        animator?.SetBool("isMoving", moveInput != Vector2.zero);
        
        // Movimiento solo actualiza animación; el flip horizontal lo maneja PlayerFacing según el mouse
    }
}
