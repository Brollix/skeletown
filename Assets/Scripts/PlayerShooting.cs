using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : Player
{
    [Header("Shooting")]
    [SerializeField] private float shootCooldown = 0.3f;
    [SerializeField] private GameObject arrowTemplate;
    [SerializeField] private float damage = 1f; // Damage dealt by each arrow
    
    private float cooldownTimer;
    private BowController bowController;

    protected override void Awake()
    {
        base.Awake();
        bowController = GetComponentInChildren<BowController>();
        
        if (arrowTemplate == null)
            Debug.LogError("Assign the arrow prefab in the Inspector!");
    }

    private void Update()
    {
        if (PauseManager.GamePaused) return;
        
        cooldownTimer -= Time.deltaTime;

        if (Mouse.current.leftButton.wasPressedThisFrame && cooldownTimer <= 0f)
        {
            Shoot();
            cooldownTimer = shootCooldown;
        }
    }

    private void Shoot()
    {
        if (bowController == null || arrowTemplate == null) 
        {
            if (bowController == null) Debug.LogError("No BowController found!");
            if (arrowTemplate == null) Debug.LogError("No arrow template assigned!");
            return;
        }

        Vector2 mousePos = GetMousePosition();
        // Get direction from player to mouse (for aiming)
        Vector2 aimDirection = (mousePos - (Vector2)transform.position).normalized;
        
        // Create arrow at bow's position
        GameObject arrow = Instantiate(arrowTemplate, bowController.transform.position, Quaternion.identity);
        arrow.SetActive(true);
        
        if (arrow.TryGetComponent(out Arrow arrowScript))
        {
            // Set the direction based on player-to-mouse aiming, but spawn at bow position
            arrowScript.setDirection(aimDirection);

            // Pass player damage into the arrow
            arrowScript.SetDamage(damage);
        }
    }
}
