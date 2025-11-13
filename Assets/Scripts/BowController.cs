using UnityEngine;
using UnityEngine.InputSystem;

<<<<<<< Updated upstream
public class BowController : MonoBehaviour {
	[SerializeField] private Camera mainCamera;
	[SerializeField] private Transform playerTransform;
	[SerializeField] private PlayerMovement playerMovement;
	[SerializeField] private float radius = 0.5f; // distance from bow to player
=======
public class BowController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private float radius = 0.5f; // distancia del arco al jugador
>>>>>>> Stashed changes

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void Update()
    {
        RotateAroundPlayer();
    }

    private void RotateAroundPlayer()
    {
        float zDist = Mathf.Abs(mainCamera.transform.position.z - playerTransform.position.z);
        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(
            new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, zDist)
        );

<<<<<<< Updated upstream
		// mouse direction from player
		Vector2 dir = (mouseWorld - playerTransform.position).normalized;

		// bow position orbiting around player
		transform.position = playerTransform.position + (Vector3) (dir * radius);

		// bow rotation facing mouse
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

		// if player faces left, invert angle
		if (!playerMovement.IsFacingRight())
			angle += 180f;
=======
        // dirección del mouse desde el jugador
        Vector2 dir = (mouseWorld - playerTransform.position).normalized;

        // posición del arco orbitando alrededor del jugador
        transform.position = playerTransform.position + (Vector3)(dir * radius);

        // rotación del arco mirando hacia el mouse
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // si el jugador mira a la izquierda, invertí el ángulo
        if (!playerMovement.IsFacingRight())
            angle += 180f;
>>>>>>> Stashed changes

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}