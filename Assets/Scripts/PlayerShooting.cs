using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : Player
{
    [Header("Shooting")]
    [SerializeField] private float shootCooldown = 0.3f;
    [SerializeField] private GameObject arrowTemplate;
    private float damage => UpgradeManager.Instance?.Damage ?? 1f;
    
    private float cooldownTimer;
    private BowController bowController;

    protected override void Awake()
    {
        base.Awake();
        bowController = GetComponentInChildren<BowController>();
    }

    private void Update()
    {
        if (PauseManager.GamePaused) return;
        
        cooldownTimer -= Time.deltaTime;

        if (input != null && input.AttackAction != null && input.AttackAction.WasPerformedThisFrame() && cooldownTimer <= 0f)
        {
            Shoot();
            cooldownTimer = shootCooldown;
        }
    }

    public static event System.Action OnShoot;

    private void Shoot()
    {
        OnShoot?.Invoke();

        if (bowController == null || arrowTemplate == null) 
        {
            return;
        }

        Vector2 aimDirection = GetAimDirection();
        
        GameObject arrow = Instantiate(arrowTemplate, bowController.transform.position, Quaternion.identity);
        arrow.SetActive(true);

        if (arrow.TryGetComponent(out Arrow arrowScript))
        {
            arrowScript.setDirection(aimDirection);
            arrowScript.SetDamage(damage);
        }
    }
}
