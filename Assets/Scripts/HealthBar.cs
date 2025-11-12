using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [Header("Referencia del enemigo")]
    public Enemy targetEnemy;  // Asignar en el inspector o por script

    [Header("Barra visual")]
    public Transform bar;      // El transform gráfico hijo que se escala

    private float maxHealth;

    void Start()
    {
        if (targetEnemy == null)
            targetEnemy = GetComponentInParent<Enemy>();
        if (targetEnemy != null)
            maxHealth = targetEnemy.health;
        else
            Debug.LogError("HealthBar: No se encontró Enemy en el objeto asignado.");
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
