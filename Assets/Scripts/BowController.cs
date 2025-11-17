using UnityEngine;
using UnityEngine.InputSystem;


public class BowController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform playerTransform;
    private PlayerFacing playerFacing;
    [SerializeField] private float radius = 0.5f; // distancia del arco al jugador

    private void Start()
    {
        Time.timeScale = 1f; // safety reset
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

        // dirección del mouse desde el jugador
        Vector2 dir = (mouseWorld - playerTransform.position).normalized;

        // posición del arco orbitando alrededor del jugador
        transform.position = playerTransform.position + (Vector3)(dir * radius);

        // ángulo mirando hacia el mouse
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        
        // rotación del arco: solo depende del mouse, no del flip del jugador
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
