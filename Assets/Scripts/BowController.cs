using UnityEngine;
using UnityEngine.InputSystem;

// Handles bow position and rotation around the player based on mouse position.
public class BowController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform playerTransform;
    private PlayerFacing playerFacing;
    [SerializeField] private float radius = 0.5f; // Distance from the player to the bow (orbit radius)

    private void Start()
    {
        // Ensure normal time scale when entering this scene
        Time.timeScale = 1f; // Safety reset
    }

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        playerFacing = GetComponentInParent<PlayerFacing>();
        // Detach bow so it does not inherit player's scale/flip
        transform.SetParent(null, true);
    }

    private void Update()
    {
        if (PauseManager.GamePaused) return;

        RotateAroundPlayer();
    }

    private void RotateAroundPlayer()
    {
        if (mainCamera == null || playerTransform == null) return;

        float zDist = Mathf.Abs(mainCamera.transform.position.z - playerTransform.position.z);
        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(
            new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, zDist)
        );

        // Mouse direction from the player
        Vector2 dir = (mouseWorld - playerTransform.position).normalized;

        // Bow position orbiting around the player
        transform.position = playerTransform.position + (Vector3)(dir * radius);

        // Angle facing towards the mouse
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        
        // Bow rotation: depends only on the mouse, not on the player's flip
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
