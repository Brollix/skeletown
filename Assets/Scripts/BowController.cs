using UnityEngine;
using UnityEngine.InputSystem;

public class BowController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform playerTransform;
    private PlayerFacing playerFacing;
    [SerializeField] private float radius = 0.5f;

    private Player player;

    private void Start()
    {
        player = (playerFacing != null) ? playerFacing.GetComponent<Player>() : GetComponentInParent<Player>();
        if (player == null) player = Player.Instance;
    }

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        playerFacing = GetComponentInParent<PlayerFacing>();

        transform.SetParent(null, true);
    }

    private void Update()
    {
        if (PauseManager.GamePaused) return;

        RotateAroundPlayer();
    }

    private void RotateAroundPlayer()
    {
        if (playerTransform == null && player == null) return;
        
        Vector3 centerPos = (playerTransform != null) ? playerTransform.position : player.transform.position;

        Vector2 dir = Vector2.right;

        if (player != null)
        {
            dir = player.GetAimDirection();
        }
        else
        {
            float zDist = Mathf.Abs(mainCamera.transform.position.z - centerPos.z);
            Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(
                new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, zDist)
            );
            dir = (mouseWorld - centerPos).normalized;
        }

        transform.position = centerPos + (Vector3)(dir * radius);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
