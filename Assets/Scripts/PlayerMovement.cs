using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : Player
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    
    // Automatic component references
    private Animator _animator;
    private Animator animator => _animator != null ? _animator : _animator = GetComponent<Animator>();
    private Camera _mainCamera;
    private Camera mainCamera => _mainCamera != null ? _mainCamera : _mainCamera = Camera.main;

    // Movement only - shooting is handled by PlayerShooting

    private PlayerControls controls;
    private Vector2 moveInput;
    private bool facingRight = true;

    private void Start()
    {
        Time.timeScale = 1f; // safety reset
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
        if (PauseManager.GamePaused)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = moveInput * moveSpeed;
    }

    private void Update()
    {
        if (PauseManager.GamePaused) 
        {
            Stop();
            return;
        }
        
        Move(input.moveInput, moveSpeed);
        facing?.UpdateFacingDirection();
    }


    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public bool IsFacingRight()
    {
        return facingRight;
    }
}
