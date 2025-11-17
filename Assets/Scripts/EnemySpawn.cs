using UnityEngine;
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

    void Start()
    {
        FloorID id = GetComponentInParent<FloorID>();
        if (id != null) {
            floorNumber = id.floorNumber;
        }

        StartCoroutine(SpawnEnemies());
    }

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

        for (int i = 0; i < numberToSpawn; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * 0.5f;
            Vector2 spawnPos = (Vector2)spawnPoint.position + randomOffset;

            GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

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
            }

            yield return null;
        }

        if (OnEnemiesSpawned != null) {
            OnEnemiesSpawned(floorNumber, numberToSpawn);
        }
    }
}