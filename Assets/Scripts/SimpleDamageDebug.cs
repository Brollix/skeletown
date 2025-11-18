using UnityEngine;

public class SimpleDamageDebug : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("🔧 SIMPLE DAMAGE DEBUG STARTED");

        // Check Player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("❌ NO PLAYER FOUND WITH TAG 'Player'");
            return;
        }

        Debug.Log("✅ Player found");

        // Check PlayerHealth
        PlayerHealth ph = player.GetComponent<PlayerHealth>();
        if (ph == null)
        {
            Debug.LogError("❌ PLAYER HAS NO PlayerHealth COMPONENT");
        }
        else
        {
            Debug.Log($"✅ PlayerHealth found - Health: {ph.CurrentHealth}/{ph.MaxHealth}");
        }

        // Check Collider
        Collider2D col = player.GetComponent<Collider2D>();
        if (col == null)
        {
            Debug.LogError("❌ PLAYER HAS NO Collider2D");
        }
        else
        {
            Debug.Log($"✅ Collider2D found - IsTrigger: {col.isTrigger}");
        }

        // Check Rigidbody
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("❌ PLAYER HAS NO Rigidbody2D");
        }
        else
        {
            Debug.Log("✅ Rigidbody2D found");
        }

        // Check enemies
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Debug.Log($"📊 Found {enemies.Length} enemies");

        // Force damage test
        Invoke("ForceDamageTest", 1f);
    }

    private void Update()
    {
        // Check for manual damage test (press D key)
        if (Input.GetKeyDown(KeyCode.D))
        {
            ForceDamageTest();
        }
    }

    private void ForceDamageTest()
    {
        Debug.Log("💥 MANUAL DAMAGE TEST (pressed D)");

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                Debug.Log($"Applying 1 damage to player (current health: {ph.CurrentHealth})");
                ph.TakeDamage(1f);
            }
            else
            {
                Debug.LogError("Player has no PlayerHealth!");
            }
        }
        else
        {
            Debug.LogError("No player found!");
        }
    }
}