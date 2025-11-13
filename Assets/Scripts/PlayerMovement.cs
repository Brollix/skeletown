using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Animator animator;
    [SerializeField] private Camera mainCamera;

    [Header("Shooting Settings")]
    [SerializeField] private BowController bowController;  // reference to Bow
    [SerializeField] private GameObject arrowTemplate;      // reference to ArrowTemplate (disabled in scene)
    [SerializeField] private float shootCooldown = 0.3f;

    private PlayerControls controls;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private bool facingRight = true;

    private float cooldownTimer;

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
        moveInput = ctx.ReadValue<Vector2>();
        animator.SetBool("isMoving", moveInput != Vector2.zero);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
        UpdateFacingDirection();
    }

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;

        // Left mouse click -> shoot
        if (Mouse.current.leftButton.wasPressedThisFrame && cooldownTimer <= 0f)
        {
            Shoot();
            cooldownTimer = shootCooldown;
        }
    }

    private void Shoot()
    {
        if (bowController == null || arrowTemplate == null) return;

        // Get mouse world position
        float zDist = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);
        Vector3 mouseWorld3 = mainCamera.ScreenToWorldPoint(
            new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, zDist)
        );
        Vector2 mouseWorld = new Vector2(mouseWorld3.x, mouseWorld3.y);

        // Compute direction and distance from bow to mouse
        Vector2 bowPos = bowController.transform.position;
        Vector2 direction = mouseWorld - bowPos;
        float distance = direction.magnitude;

        // Prevent shooting if the mouse is too close
        float minShootDistance = 0.5f;
        if (distance < minShootDistance)
            return;

        direction.Normalize();

        // Get bow's forward direction and correct for player facing
        Vector2 bowForward = bowController.transform.right;

        // If the player is facing left, invert forward
        if (!IsFacingRight())
            bowForward *= -1f;

        float dot = Vector2.Dot(direction, bowForward);

        // Require the mouse to be generally in front of the bow
        float minDotToShoot = 0.2f;
        if (dot < minDotToShoot)
            return;

        // Spawn arrow and set direction
        GameObject arrow = Instantiate(arrowTemplate, bowController.transform.position, Quaternion.identity);
        arrow.SetActive(true);

        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
            arrowScript.setDirection(direction);
    }






    private void UpdateFacingDirection()
    {
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