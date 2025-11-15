using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : Player
{
    [Header("Shooting")]
    [SerializeField] private GameObject arrowTemplate;
    [SerializeField] private float shootCooldown = 0.3f;

    private BowController bowController;
    private float cooldownTimer;

    protected override void Awake()
    {
        base.Awake();
        bowController = GetComponentInChildren<BowController>();
    }

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (Mouse.current.leftButton.wasPressedThisFrame && cooldownTimer <= 0f)
        {
            Shoot();
            cooldownTimer = shootCooldown;
        }
    }

    private void Shoot()
    {
        if (bowController == null || arrowTemplate == null) return;

        Vector2 mousePos = GetMousePosition();
        Vector2 bowPos = bowController.transform.position;
        Vector2 direction = mousePos - bowPos;
        
        if (direction.magnitude < 0.5f) return;
        
        direction.Normalize();
        
        // Check if shooting direction is valid
        Vector2 bowForward = bowController.transform.right;
        if (!IsFacingRight()) bowForward *= -1f;
        
        if (Vector2.Dot(direction, bowForward) < 0.2f) return;

        // Create arrow
        GameObject arrow = Instantiate(arrowTemplate, bowPos, Quaternion.identity);
        arrow.SetActive(true);
        
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
            arrowScript.setDirection(direction);
    }
}
