using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;     
    public int numberToSpawn = 5;
    public float spawnRadius = 1f;
    public float spawnDelay = 2f;

    void Start()
    {
        StartCoroutine(SpawnEnemiesOverTime());
    }

    IEnumerator SpawnEnemiesOverTime()
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            // Pick a spawn position around the spawner
            Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;

            // Create the enemy
            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            // Assign player reference so enemy can follow
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
                enemyScript.player = player;

            // Wait before spawning the next one
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
