using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : Player
{
    public Vector2 moveInput { get; private set; }
    public Vector2 lookInput { get; private set; }
    public InputAction AttackAction { get; private set; }
    public InputAction DashAction { get; private set; }
    public InputAction PauseAction { get; private set; }

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
        controls.UI.Enable();

        controls.Player.Move.performed += OnMove;
        controls.Player.Move.canceled += OnMove;

        controls.Player.Look.performed += OnLook;
        controls.Player.Look.canceled += OnLook;

        AttackAction = controls.Player.Attack;

        DashAction = controls.Player.Dash;
        controls.Player.Dash.performed += OnDash;

        PauseAction = controls.UI.PauseToggle;
    }

    private void OnDisable()
    {
        if (controls != null)
        {
            controls.Player.Move.performed -= OnMove;
            controls.Player.Move.canceled -= OnMove;
            controls.Player.Look.performed -= OnLook;
            controls.Player.Look.canceled -= OnLook;
            controls.Player.Dash.performed -= OnDash;
            controls.Player.Disable();
            controls.UI.Disable();
        }
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        animator?.SetBool("isMoving", moveInput != Vector2.zero);
    }

    public bool DashTriggered { get; private set; }

    private void OnDash(InputAction.CallbackContext ctx)
    {
        DashTriggered = true;
    }

    public void ResetDashTrigger()
    {
        DashTriggered = false;
    }

    private void OnLook(InputAction.CallbackContext ctx)
    {
        if (ctx.control.device is Pointer) return;

        lookInput = ctx.ReadValue<Vector2>();
    }

    public Vector2 CurrentAimDirection { get; private set; } = Vector2.right;
    private bool usingGamepad = false;
    private Vector2 lastGamepadDir = Vector2.right;

    private void Update()
    {
        if (PauseManager.GamePaused) return;

        if (lookInput.sqrMagnitude > 0.01f)
        {
            usingGamepad = true;
            lastGamepadDir = lookInput.normalized;
        }

        if (Mouse.current != null && Mouse.current.delta.ReadValue().sqrMagnitude > 0.5f)
        {
            usingGamepad = false;
        }

        if (usingGamepad)
        {
            CurrentAimDirection = lastGamepadDir;
        }
        else
        {
            Vector2 mousePos = GetMousePosition();
            CurrentAimDirection = (mousePos - (Vector2)transform.position).normalized;
        }
    }
}
