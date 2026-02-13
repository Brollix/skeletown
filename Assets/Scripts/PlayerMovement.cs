using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : Player
{
    [Header("Movement")]
    private float moveSpeed => (UpgradeManager.Instance?.Speed ?? 5f) * (CheatManager.Instance != null && CheatManager.Instance.IsSuperSpeed ? CheatManager.Instance.SpeedMultiplier : 1f);
    
    private Animator _animator;
    private Animator animator => _animator != null ? _animator : _animator = GetComponent<Animator>();
    private Camera _mainCamera;
    private Camera mainCamera => _mainCamera != null ? _mainCamera : _mainCamera = Camera.main;

    private PlayerControls controls;
    private Vector2 moveInput;

    private void Start()
    {
    }

    private void Awake()
    {
        base.Awake();
        controls = new PlayerControls();
        _animator = GetComponent<Animator>();
        _mainCamera = Camera.main;
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
        if (PauseManager.GamePaused)
        {
            moveInput = Vector2.zero;
            if (animator != null)
                animator.SetBool("isMoving", false);
            return;
        }

        moveInput = ctx.ReadValue<Vector2>();
        if (animator != null)
            animator.SetBool("isMoving", moveInput != Vector2.zero);
    }

    private void FixedUpdate()
    {
        if (PauseManager.GamePaused || isDashing) return;

        if (rb != null)
        {
            Vector2 movement = moveInput.normalized * moveSpeed;
            rb.linearVelocity = new Vector2(movement.x, movement.y);
        }

        if (animator != null)
        {
            bool isMoving = moveInput.magnitude > 0.1f;
            animator.SetBool("isMoving", isMoving);
        }
    }

    private void Update()
    {
        if (PauseManager.GamePaused) 
        {
            Stop();
            return;
        }
        
        facing?.UpdateFacingDirection();

        if (input != null && input.DashTriggered)
        {
            input.ResetDashTrigger();
            StartDash();
        }
    }

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    private float lastDashTime = -999f;
    private bool isDashing = false;

    private void StartDash()
    {
        if (isDashing || Time.time < lastDashTime + dashCooldown) return;
        
        StartCoroutine(DashCoroutine());
    }

    private System.Collections.IEnumerator DashCoroutine()
    {
        isDashing = true;
        lastDashTime = Time.time;

        Vector2 dashDir = moveInput.normalized;
        if (dashDir == Vector2.zero)
        {
            dashDir = input.CurrentAimDirection;
        }

        float elapsed = 0f;
        while (elapsed < dashDuration)
        {
            if (PauseManager.GamePaused)
            {
                isDashing = false;
                yield break;
            }

            rb.linearVelocity = dashDir * dashSpeed;
            elapsed += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
    }
}
