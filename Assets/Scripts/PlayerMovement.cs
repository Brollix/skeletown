using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private Animator animator;
	[SerializeField] private Camera mainCamera;

	private PlayerControls controls;
	private Vector2 moveInput;
	private Rigidbody2D rb;
	private bool facingRight = true;

	private void Awake() {
		controls = new PlayerControls();
		rb = GetComponent<Rigidbody2D>();
		if (mainCamera == null)
			mainCamera = Camera.main;
	}

	private void OnEnable() {
		controls.Player.Enable();
		controls.Player.Move.performed += OnMove;
		controls.Player.Move.canceled += OnMove;
	}

	private void OnDisable() {
		controls.Player.Move.performed -= OnMove;
		controls.Player.Move.canceled -= OnMove;
		controls.Player.Disable();
	}

	private void OnMove(InputAction.CallbackContext ctx) {
		moveInput = ctx.ReadValue<Vector2>();
		animator.SetBool("isMoving", moveInput != Vector2.zero);
	}

	private void FixedUpdate() {
		rb.linearVelocity = moveInput * moveSpeed;
		UpdateFacingDirection();
	}

	private void UpdateFacingDirection() {
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

	private void Flip() {
		facingRight = !facingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

	public bool IsFacingRight() => facingRight;
}
