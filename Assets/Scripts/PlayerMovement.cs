using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : Player
{
    [Header("Movement")]
    private float moveSpeed => UpgradeManager.Instance?.Speed ?? 5f;
    
    // Automatic component references
    private Animator _animator;
    private Animator animator => _animator != null ? _animator : _animator = GetComponent<Animator>();
    private Camera _mainCamera;
    private Camera mainCamera => _mainCamera != null ? _mainCamera : _mainCamera = Camera.main;

    // Movement only - shooting is handled by PlayerShooting

    private PlayerControls controls;
    private Vector2 moveInput;

    private void Start()
    {
        Time.timeScale = 1f; // safety reset
        Debug.Log($"🏃 Player speed: {moveSpeed} (base: 5, upgrades: {UpgradeManager.Instance?.Speed ?? 5f})");
    }


    private void Awake()
    {
        base.Awake();
        controls = new PlayerControls();
        _animator = GetComponent<Animator>();
        _mainCamera = Camera.main;
        Time.timeScale = 1f; // Ensure time is running
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
        if (PauseManager.GamePaused) return;

        // Move the player
        if (rb != null)
        {
            Vector2 movement = moveInput.normalized * moveSpeed;
            rb.linearVelocity = new Vector2(movement.x, movement.y);
        }

        // Update animation
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
        
        // Movement is handled in FixedUpdate
        facing?.UpdateFacingDirection();
    }
}
