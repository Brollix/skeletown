using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Dictionary<int, int> enemiesRemaining = new Dictionary<int, int>();
    private Dictionary<int, List<Door>> doorsPerFloor = new Dictionary<int, List<Door>>();


    //Initializes the Singleton instance. Singleton so that it can be accessible from anywhere, and it centralizes logic so enemies and doors don't need to know about each other directly.
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public bool IsGameOver = false;


    //Resets the game state, clearing enemy and door tracking.
    public void ResetGame()
    {
        IsGameOver = false;
        enemiesRemaining.Clear();
        doorsPerFloor.Clear();
        Debug.Log("GameManager: Game state reset.");
    }


    //Registers a door for a specific floor and ensures it is closed if enemies are present.
    public void RegisterDoor(int floor, Door door)
    {
        if (!doorsPerFloor.ContainsKey(floor))
        {
            doorsPerFloor.Add(floor, new List<Door>());
        }

        doorsPerFloor[floor].Add(door);

        if (enemiesRemaining.ContainsKey(floor) && enemiesRemaining[floor] > 0)
        {
            door.SetOpen(false);
        }
    }


    //Adds the corresponding number of enemies to the specific floor's count and locks the doors. It forces doors to close when enemies are added to avoid any open doors on startup.
    public void AddEnemies(int floor, int amount)
    {
        if (!enemiesRemaining.ContainsKey(floor))
        {
            enemiesRemaining.Add(floor, 0);
        }

        enemiesRemaining[floor] += amount;

        if (doorsPerFloor.ContainsKey(floor))
        {
            foreach (Door d in doorsPerFloor[floor])
            {
                if (d != null)
                {
                    d.SetOpen(false);
                }
            }
        }
    }


    //Notifies the gameManager that an enemy on a specific floor has died, opening doors once all are dead.
    public void EnemyDied(int floor)
    {
        if (!enemiesRemaining.ContainsKey(floor))
        {
            
            return;
        }

        enemiesRemaining[floor] = Mathf.Max(0, enemiesRemaining[floor] - 1);

        if (enemiesRemaining[floor] == 0)
        {

            if (doorsPerFloor.ContainsKey(floor))
            {
                foreach (Door d in doorsPerFloor[floor])
                {
                    if (d != null)
                    {
                        d.SetOpen(true);
                    }
                }
            }
        }
    }


    //This checks for cheat inputs (F1, F2).
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.f1Key.wasPressedThisFrame)
        {
            KillAllEnemiesExceptBoss();
        }

        if (Keyboard.current != null && Keyboard.current.f2Key.wasPressedThisFrame)
        {
            MaxStatsCheat();
        }
    }


    //Kills all enemies except the final boss and teleports the player.
    private void KillAllEnemiesExceptBoss()
    {
        Debug.Log("CHEAT: Killing all enemies except Floor 20 Boss...");
        
        List<Enemy> enemiesToKill = new List<Enemy>(Enemy.ActiveEnemies);

        int count = 0;
        foreach (Enemy e in enemiesToKill)
        {
            if (e == null) continue;

            if (e.floorNumber == 20 && e.isBoss)
            {
                continue;
            }

            e.TakeDamage(999999f);
            count++;
        }
        
        Debug.Log($"CHEAT: Killed {count} enemies. Teleporting to Floor 20...");

        TeleportToFloor(20);
    }


    //Maxes out all upgrade stats and heals the player.
    private void MaxStatsCheat()
    {
        Debug.Log("CHEAT: Maxing Stats...");
        if (UpgradeManager.Instance != null)
        {
            UpgradeManager.Instance.CheatMaxOutStats();
        }

        if (Player.Instance != null)
        {
            PlayerHealth health = Player.Instance.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.Heal(9999f);
            }
        }
    }


    //Moves the player to the spawn point of the specified floor, in this case Floor 20.
    private void TeleportToFloor(int floor)
    {
        EnemySpawn[] spawners = FindObjectsOfType<EnemySpawn>();
        foreach (EnemySpawn spawner in spawners)
        {   
            FloorID textID = spawner.GetComponentInParent<FloorID>();
            if (textID != null && textID.floorNumber == floor)
            {
                if (Player.Instance != null)
                {
                    Vector3 targetPos = spawner.spawnPoint != null ? spawner.spawnPoint.position : spawner.transform.position;
                    Player.Instance.transform.position = targetPos;
                    Debug.Log($"CHEAT: Teleported to Floor {floor} at {targetPos}");
                    return;
                }
            }
        }
        Debug.LogWarning($"CHEAT: Could not find spawner for Floor {floor}");
    }
}
