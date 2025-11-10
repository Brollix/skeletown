using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
	[SerializeField] private float moveSpeed = 5f;

	private PlayerControls controls;
	private Vector2 moveInput;
	private Rigidbody2D rb;

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
	}

	private void FixedUpdate() {
		rb.linearVelocity = moveInput * moveSpeed;
	}
}
