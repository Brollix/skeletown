using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;   // Enemy prefab to spawn
    public Transform player;         // Player reference for enemies
    public Transform spawnPoint;     // Single spawn point
    public int numberToSpawn = 5;    // Total enemies to spawn

    void Start()
    {
        SpawnAllEnemies();
    }

    void SpawnAllEnemies()
    {
        if (enemyPrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("Enemy prefab or spawn point not set.");
            return;
        }

        for (int i = 0; i < numberToSpawn; i++)
        {
            // Instantiate the enemy at the spawn point
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

            // Assign player reference so the enemy can follow
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
                enemyScript.player = player;
        }
    }
}
