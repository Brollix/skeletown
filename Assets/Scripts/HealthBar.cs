using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [Header("Enemy Reference")]
    public Enemy targetEnemy;  // Assign in inspector or by script

    [Header("Visual Bar")]
    public Transform bar;      // Child graphic transform that scales

    private float maxHealth;

    void Start()
    {
        if (targetEnemy == null)
            targetEnemy = GetComponentInParent<Enemy>();
        if (targetEnemy != null)
            maxHealth = targetEnemy.health;
        else
            Debug.LogError("HealthBar: Enemy not found on assigned object.");
    }

    void Update()
    {
        if (targetEnemy != null && bar != null)
        {
            float percent = Mathf.Clamp01(targetEnemy.health / maxHealth);
            bar.localScale = new Vector3(percent, 1f, 1f);
        }
    }
}
