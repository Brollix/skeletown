using UnityEngine;

public class DamageTest : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("🧪 DAMAGE TEST STARTED");

        // Verificar si el Player existe
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Debug.Log("✅ Player found with tag 'Player'");

            // Verificar si tiene PlayerHealth
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                Debug.Log($"✅ Player has PlayerHealth component - Current Health: {playerHealth.CurrentHealth}/{playerHealth.MaxHealth}");
            }
            else
            {
                Debug.LogError("❌ Player does NOT have PlayerHealth component!");
            }

            // Verificar Rigidbody2D
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Debug.Log("✅ Player has Rigidbody2D");
            }
            else
            {
                Debug.LogError("❌ Player does NOT have Rigidbody2D!");
            }

            // Verificar Collider2D
            Collider2D collider = player.GetComponent<Collider2D>();
            if (collider != null)
            {
                Debug.Log("✅ Player has Collider2D");
                Debug.Log($"   - Is Trigger: {collider.isTrigger}");
                Debug.Log($"   - Layer: {LayerMask.LayerToName(player.layer)}");
            }
            else
            {
                Debug.LogError("❌ Player does NOT have Collider2D!");
            }
        }
        else
        {
            Debug.LogError("❌ No GameObject found with tag 'Player'!");
        }

        // Verificar enemigos
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Debug.Log($"📊 Found {enemies.Length} enemies in scene");

        foreach (Enemy enemy in enemies)
        {
            Debug.Log($"   - Enemy: {enemy.gameObject.name}, Damage: {enemy.damage}, Floor: {enemy.floorNumber}");
        }

        Debug.Log("🧪 DAMAGE TEST COMPLETED - Check logs above for issues");

        // Test manual de daño
        Invoke("TestManualDamage", 2f);
    }

    private void TestManualDamage()
    {
        Debug.Log("🔥 TESTING MANUAL DAMAGE");

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                Debug.Log("💥 Applying 1 test damage to player...");
                playerHealth.TakeDamage(1f);
            }
        }
    }
}