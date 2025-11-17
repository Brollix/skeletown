using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : Player
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Animator animator;
    [SerializeField] private Camera mainCamera;

    [Header("Shooting Settings")]
    [SerializeField] private BowController bowController;
    [SerializeField] private GameObject arrowTemplate;
    [SerializeField] private float shootCooldown = 0.3f;

    private PlayerControls controls;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private bool facingRight = true;

    private float cooldownTimer;

    private void Start()
    {
        Time.timeScale = 1f; // safety reset
    }


    private void Awake()
    {
        controls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        if (mainCamera == null)
            mainCamera = Camera.main;
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
            animator.SetBool("isMoving", false);
            return;
        }

        moveInput = ctx.ReadValue<Vector2>();
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

        UpdateFacingDirection();
    }

    private void Update()
    {
        if (PauseManager.GamePaused) return;

        cooldownTimer -= Time.deltaTime;

        if (Mouse.current.leftButton.wasPressedThisFrame && cooldownTimer <= 0f)
        {
            Move(input.moveInput, moveSpeed);
        }
        else
        {
            Stop();
        }
        
        facing?.UpdateFacingDirection();
    }

    private void Shoot()
    {
        if (bowController == null || arrowTemplate == null) return;

        float zDist = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);
        Vector3 mouseWorld3 = mainCamera.ScreenToWorldPoint(
            new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, zDist)
        );
        Vector2 mouseWorld = new Vector2(mouseWorld3.x, mouseWorld3.y);

        Vector2 bowPos = bowController.transform.position;
        Vector2 direction = mouseWorld - bowPos;
        float distance = direction.magnitude;

        float minShootDistance = 0.5f;
        if (distance < minShootDistance)
            return;

        direction.Normalize();

        Vector2 bowForward = bowController.transform.right;

        if (!IsFacingRight())
            bowForward *= -1f;

        float dot = Vector2.Dot(direction, bowForward);

        float minDotToShoot = 0.2f;
        if (dot < minDotToShoot)
            return;

        GameObject arrow = Instantiate(arrowTemplate, bowController.transform.position, Quaternion.identity);
        arrow.SetActive(true);

        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
            arrowScript.setDirection(direction);
    }

    private void UpdateFacingDirection()
    {
        if (PauseManager.GamePaused) return;     // <-- FIX FOR PLAYER FLIPPING

        float zDist = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);
        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(
            new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, zDist)
        );

        bool mouseRight = mouseWorld.x > transform.position.x;

        if (mouseRight && !facingRight)
            Flip();
        else if (!mouseRight && facingRight)
            Flip();
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
