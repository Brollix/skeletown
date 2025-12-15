using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Dictionary<int, int> enemiesRemaining = new Dictionary<int, int>();
    private Dictionary<int, List<Door>> doorsPerFloor = new Dictionary<int, List<Door>>();


    // Initializes the Singleton instance.
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


    // Resets the game state, clearing enemy and door tracking.
    public void ResetGame()
    {
        IsGameOver = false; // Reset game over state
        enemiesRemaining.Clear();
        doorsPerFloor.Clear();
        Debug.Log("GameManager: Game state reset.");
    }



    // ------------------------------------------------------
    // REGISTER DOORS
    // ------------------------------------------------------

    // Registers a door for a specific floor and ensures it is closed if enemies are present.
    public void RegisterDoor(int floor, Door door)
    {
        if (!doorsPerFloor.ContainsKey(floor))
        {
            doorsPerFloor.Add(floor, new List<Door>());
        }

        doorsPerFloor[floor].Add(door);

        // Fix: If enemies are already registered for this floor, ensure the door is closed.
        if (enemiesRemaining.ContainsKey(floor) && enemiesRemaining[floor] > 0)
        {
            door.SetOpen(false);
        }
    }


    // ------------------------------------------------------
    // REGISTER ENEMIES FROM SPAWNER
    // ------------------------------------------------------

    // Adds a number of enemies to a specific floor's count and locks the doors.
    public void AddEnemies(int floor, int amount)
    {
        if (!enemiesRemaining.ContainsKey(floor))
        {
            enemiesRemaining.Add(floor, 0);
        }

        enemiesRemaining[floor] += amount;

        // Fix: Force doors closed when enemies are added
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

    // ------------------------------------------------------
    // CALLED BY ENEMY WHEN IT DIES
    // ------------------------------------------------------

    // Notifies the manager that an enemy on a specific floor has died, potentially opening doors.
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

    // ------------------------------------------------------
    // CHEAT: F1 TO KILL ALL ENEMIES EXCEPT FINAL BOSS
    // ------------------------------------------------------

    // Checks for debug inputs (F1, F2).
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


    // Kills all enemies except the final boss and teleports the player.
    private void KillAllEnemiesExceptBoss()
    {
        Debug.Log("CHEAT: Killing all enemies except Floor 20 Boss...");
        
        // Create a copy of the list to avoid modification errors during iteration
        List<Enemy> enemiesToKill = new List<Enemy>(Enemy.ActiveEnemies);

        int count = 0;
        foreach (Enemy e in enemiesToKill)
        {
            if (e == null) continue;

            // Skip Floor 20 Boss
            if (e.floorNumber == 20 && e.isBoss)
            {
                continue;
            }

            e.TakeDamage(999999f);
            count++;
        }
        
        Debug.Log($"CHEAT: Killed {count} enemies. Teleporting to Floor 20...");

        // Teleport to Floor 20
        TeleportToFloor(20);
    }


    // Maxes out all upgrade stats and heals the player.
    private void MaxStatsCheat()
    {
        Debug.Log("CHEAT: Maxing Stats...");
        if (UpgradeManager.Instance != null)
        {
            UpgradeManager.Instance.CheatMaxOutStats();
        }

        // Heal player to new max health
        if (Player.Instance != null)
        {
            PlayerHealth health = Player.Instance.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.Heal(9999f); // Full heal
            }
        }
    }


    // Moves the player to the spawn point of the specified floor.
    private void TeleportToFloor(int floor)
    {
        EnemySpawn[] spawners = FindObjectsOfType<EnemySpawn>();
        foreach (EnemySpawn spawner in spawners)
        {
            // Assuming the script exposes floorNumber publicly, or we access via reflection/GetComponent
            // Based on previous file read, floorNumber is private but set from FloorID.
            // However, EnemySpawn has 'floorNumber' as private field.
            // But wait, I saw 'public int floorNumber' in Door.cs, but in EnemySpawn.cs it was 'private int floorNumber'.
            // Let's check FloorID component on the spawner's parent instead, which is safer.
            
            FloorID textID = spawner.GetComponentInParent<FloorID>();
            if (textID != null && textID.floorNumber == floor)
            {
                if (Player.Instance != null)
                {
                    // Use spawner position or its defined spawn point
                    Vector3 targetPos = spawner.spawnPoint != null ? spawner.spawnPoint.position : spawner.transform.position;
                    Player.Instance.transform.position = targetPos;
                    Debug.Log($"CHEAT: Teleported to Floor {floor} at {targetPos}");
                    return;
                }
            }
        }
        Debug.LogWarning($"CHEAT: Could not find spawner for Floor {floor}");
    }

    // ------------------------------------------------------
    // DEBUG: PRINT FLOOR STATES
    // ------------------------------------------------------
    // public void PrintFloorStates()
    // {
    //     Debug.Log("----- FLOOR STATES -----");

    //     // Collect all floors that exist either in doors or enemies
    //     HashSet<int> allFloors = new HashSet<int>();
    //     foreach (var floor in doorsPerFloor.Keys) allFloors.Add(floor);
    //     foreach (var floor in enemiesRemaining.Keys) allFloors.Add(floor);

    //     foreach (int floor in allFloors)
    //     {
    //         int enemies = enemiesRemaining.ContainsKey(floor) ? enemiesRemaining[floor] : 0;

    //         string doorStates = "";
    //         if (doorsPerFloor.ContainsKey(floor))
    //         {
    //             foreach (Door d in doorsPerFloor[floor])
    //             {
    //                 if (d != null)
    //                     doorStates += d.isOpen ? "OPEN " : "CLOSED ";
    //             }
    //         }
    //         else
    //         {
    //             doorStates = "No doors";
    //         }

    //         Debug.Log($"Floor {floor}: Enemies remaining = {enemies}, Doors = {doorStates}");
    //     }

    //     Debug.Log("------------------------");
    // }
}
