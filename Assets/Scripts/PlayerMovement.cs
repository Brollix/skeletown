using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private Animator animator;

	private PlayerControls controls;
	private Vector2 moveInput;
	private Rigidbody2D rb;
	private bool facingRight = true;

	private void Awake() {
		controls = new PlayerControls();
		rb = GetComponent<Rigidbody2D>();
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

		// Flip solo si cambia de dirección
		if (moveInput.x > 0 && !facingRight) {
			Flip();
		} else if (moveInput.x < 0 && facingRight) {
			Flip();
		}

		animator.SetBool("isWalking", moveInput != Vector2.zero);
	}

	private void FixedUpdate() {
		rb.linearVelocity = moveInput * moveSpeed;
	}

	private void Flip() {
		facingRight = !facingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}
}
