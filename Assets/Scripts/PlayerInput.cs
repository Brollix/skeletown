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
        
        // Update bow rotation based on movement direction
        if (moveInput != Vector2.zero && bowTransform != null)
        {
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            bowTransform.rotation = Quaternion.Euler(0f, 0f, angle);
            
            // Handle player sprite flipping
            if (moveInput.x != 0)
            {
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Sign(moveInput.x) * Mathf.Abs(scale.x);
                transform.localScale = scale;
                
                // Keep bow's scale positive to prevent flipping
                Vector3 bowScale = bowTransform.localScale;
                bowScale.x = Mathf.Abs(bowScale.x);
                bowTransform.localScale = bowScale;
            }
        }
    }
}
