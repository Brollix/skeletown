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


    
    private int floorNumber;

    [Header("Boss Settings")]
    public bool isBossSpawner = false;



    // Identifies floor number and begins spawning.
    void Start()
    {
        FloorID id = GetComponentInParent<FloorID>();
        if (id != null)
        {
            floorNumber = id.floorNumber;
        }
        else
        {
            Debug.LogWarning("EnemySpawn: No FloorID found in parent!");
        }
        
        StartCoroutine(SpawnEnemies());
    }


    // Spawns enemies sequentially and registers them with the manager.
    IEnumerator SpawnEnemies()
    {
        // DIRECT REGISTRATION TO FIX RACE CONDITION
        // We register BEFORE spawning so that if the player kills them instantly,
        // the Game Manager already knows they exist.
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddEnemies(floorNumber, numberToSpawn);
        }
        else
        {
            Debug.LogError("EnemySpawn: GameManager not found! Doors will not open!");
        }

        // ... (existing checks)

        for (int i = 0; i < numberToSpawn; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * 0.5f;
            Vector2 spawnPos = (Vector2)spawnPoint.position + randomOffset;

            // Instantiating as child of EnemySpawn to ensure destruction on scene unload
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity, transform);

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

            yield return null;
        }


    }
}