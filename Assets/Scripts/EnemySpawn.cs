using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour
{
    [Header("Spawning")]
    public GameObject enemyPrefab;   // Enemy prefab to spawn
    public Transform player;         // Player reference for enemies
    public Transform spawnPoint;     // Single spawn point
    public int numberToSpawn = 5;    // Total enemies to spawn

    [Header("Enemy Stats Override")]
    public float enemySpeed = 2f;
    public float enemyHealth = 10f;
    public float enemyDamage = 2f;
    public float separationRadius = 1f;
    public float separationForce = 2f;

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
            Vector2 offset = Random.insideUnitCircle * 0.5f;
            Vector2 spawnPos = (Vector2)spawnPoint.position + offset;

            GameObject enemyObj = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            Enemy enemyScript = enemyObj.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.player = player;

                // 🟢 Apply custom stats from this spawner instance
                enemyScript.speed = enemySpeed;
                enemyScript.health = enemyHealth;
                enemyScript.damage = enemyDamage;
                enemyScript.separationRadius = separationRadius;
                enemyScript.separationForce = separationForce;
            }

            yield return null;
        }
    }
}
