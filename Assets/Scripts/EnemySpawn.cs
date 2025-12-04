using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public int numberToSpawn = 5;

    public float enemySpeed = 2f;
    public float enemyHealth = 10f;
    public float enemyDamage = 2f;
    public float separationRadius = 1f;
    public float separationForce = 2f;

    public static event System.Action<int, int> OnEnemiesSpawned;
    
    private int floorNumber;

    [Header("Boss Settings")]
    public bool isBossSpawner = false;


    IEnumerator Start()
    {
        // Wait until this scene is the active scene
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == gameObject.scene.name);

        // Wait until old scenes are unloaded (Boot + ThisScene = 2 scenes)
        // This prevents collisions with objects from the previous scene (like MainMenu)
        yield return new WaitUntil(() => SceneManager.sceneCount <= 2);

        FloorID id = GetComponentInParent<FloorID>();
        if (id != null) {
            floorNumber = id.floorNumber;
        }

        StartCoroutine(SpawnEnemies());
    }

    [Header("Spawn Settings")]
    public LayerMask obstacleLayer; // Assign "Environment" or "Walls" layer here
    public float spawnRadius = 3.0f;

    IEnumerator SpawnEnemies()
    {
        if (enemyPrefab == null) {
            Debug.LogWarning("Enemy prefab missing");
            yield break;
        }

        if (spawnPoint == null) {
            Debug.LogWarning("Spawn point missing");
            yield break;
        }

        int spawnedCount = 0;
        int attempts = 0;
        int maxAttempts = numberToSpawn * 10; // More attempts

        while (spawnedCount < numberToSpawn && attempts < maxAttempts)
        {
            attempts++;
            
            // Random point within radius
            Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
            Vector2 spawnPos = (Vector2)spawnPoint.position + randomOffset;

            // Check if position is valid (not inside a wall)
            // We check for a collider with the obstacle layer
            Collider2D hit = Physics2D.OverlapCircle(spawnPos, 0.4f, obstacleLayer); 
            if (hit != null)
            {
                // Hit a wall, try again
                continue;
            }

            GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            Debug.Log($"[EnemySpawn] Spawning enemy {spawnedCount+1}/{numberToSpawn} at {spawnPos}");

            Enemy e = newEnemy.GetComponent<Enemy>();
            if (e != null)
            {
                e.speed = enemySpeed;
                e.maxHealth = enemyHealth;
                e.health = enemyHealth;
                e.damage = enemyDamage;
                e.separationRadius = separationRadius;
                e.separationForce = separationForce;
                e.floorNumber = floorNumber;
                if (isBossSpawner)
                    e.isBoss = true;
            }
            else
            {
                Debug.LogError($"[EnemySpawn] Spawned object {newEnemy.name} does not have an Enemy component!");
            }

            spawnedCount++;
            yield return null; 
        }

        if (spawnedCount < numberToSpawn)
        {
            Debug.LogWarning($"[EnemySpawn] Could only spawn {spawnedCount}/{numberToSpawn} enemies after {attempts} attempts. Try increasing radius or moving the spawner.");
        }

        if (OnEnemiesSpawned != null) {
            OnEnemiesSpawned(floorNumber, spawnedCount); // Use actual spawned count, not target
            Debug.Log($"[EnemySpawn] Registered {spawnedCount} enemies with GameManager for Floor {floorNumber}");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(spawnPoint.position, spawnRadius);
        }
    }
}