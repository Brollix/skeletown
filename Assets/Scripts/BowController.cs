using UnityEngine;
using UnityEngine.InputSystem;


public class BowController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform playerTransform;
    private PlayerFacing playerFacing;
    [SerializeField] private float radius = 0.5f;


    //This method ensures the game's time scale is reset when the bow initializes, so that the bow doesn't end up frozen once the gameplay scene starts.
    private void Start()
    {
        Time.timeScale = 1f;
    }


    //This method initiaizes a reference to the camera, it initializes PlayerFacing and it detatched the bow from the player so the bow's sprite doesn't flip along with the player sprite.
    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        playerFacing = GetComponentInParent<PlayerFacing>();

        transform.SetParent(null, true);
    }

    //This method first checks if the game is paused. If it isn't, it calls the method that allows the bow to rotate, updating every frame so it loops rotation logic.
    private void Update()
    {
        if (PauseManager.GamePaused) return;

        RotateAroundPlayer();
    }


    //This method is what allows the bow to rotate around the player based on the mouse cursor position. It gets the mouse position relative to the player, then uses that to set the bow's position to orbit around the player, and it makes an angle to follow the mouse cursor that is used to make the bow rotate around.
    private void RotateAroundPlayer()
    {
        if (mainCamera == null || playerTransform == null) return;

        float zDist = Mathf.Abs(mainCamera.transform.position.z - playerTransform.position.z);
        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(
            new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, zDist)
        );

        Vector2 dir = (mouseWorld - playerTransform.position).normalized;

        transform.position = playerTransform.position + (Vector3)(dir * radius);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
