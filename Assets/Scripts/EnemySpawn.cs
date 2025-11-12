using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyPrefab;   // Enemy prefab to spawn
    public Transform player;         // Player reference for enemies
    public Transform spawnPoint;     // Single spawn point
    public int numberToSpawn = 5;    // Total enemies to spawn

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        if (enemyPrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("Enemy prefab or spawn point not set.");
            yield break;
        }

        for (int i = 0; i < numberToSpawn; i++)
        {
            // Add a small random offset so colliders don't overlap perfectly
            Vector2 offset = Random.insideUnitCircle * 0.5f;
            Vector2 spawnPos = (Vector2)spawnPoint.position + offset;

            // Instantiate the enemy
            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            // Assign player reference
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
                enemyScript.player = player;

            // Wait one frame before spawning the next enemy
            yield return null;
        }
    }
}
