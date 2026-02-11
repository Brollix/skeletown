using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : Player
{
    public Vector2 moveInput { get; private set; }
    public Vector2 lookInput { get; private set; }
    public InputAction AttackAction { get; private set; }
    public InputAction PauseAction { get; private set; }

    private PlayerControls controls;
    private Animator _animator;
    private Animator animator => _animator != null ? _animator : _animator = GetComponent<Animator>();
    private BowController _bowController;
    private Transform bowTransform => _bowController != null ? _bowController.transform : (_bowController = GetComponentInChildren<BowController>())?.transform;


    // Initializes input controls and references.
    protected override void Awake()
    {
        base.Awake();
        controls = new PlayerControls();
        _animator = GetComponent<Animator>();
        _bowController = GetComponentInChildren<BowController>();
    }


    // Enables the input system.
    private void OnEnable()
    {
        if (controls == null) controls = new PlayerControls();
        controls.Player.Enable();
        controls.UI.Enable(); // Ensure UI map is enabled for Pause

        // Movement
        controls.Player.Move.performed += OnMove;
        controls.Player.Move.canceled += OnMove;

        // Look
        controls.Player.Look.performed += OnLook;
        controls.Player.Look.canceled += OnLook;

        // Attack (Exposed for polling)
        AttackAction = controls.Player.Attack;
        
        // Pause (Exposed for polling if needed, but usually event driven)
        PauseAction = controls.UI.PauseToggle;
    }


    // Disables the input system.
    private void OnDisable()
    {
        if (controls != null)
        {
            controls.Player.Move.performed -= OnMove;
            controls.Player.Move.canceled -= OnMove;
            controls.Player.Look.performed -= OnLook;
            controls.Player.Look.canceled -= OnLook;
            controls.Player.Disable();
            controls.UI.Disable();
        }
    }


    // Callback for movement input events.
    private void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        animator?.SetBool("isMoving", moveInput != Vector2.zero);
    }

    // Callback for look input events.
    private void OnLook(InputAction.CallbackContext ctx)
    {
        // Ignore Mouse/Pointer delta because it conflicts with Mouse Position logic
        if (ctx.control.device is Pointer) return;

        lookInput = ctx.ReadValue<Vector2>();
    }
}
