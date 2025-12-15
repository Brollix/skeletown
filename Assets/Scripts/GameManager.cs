using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Dictionary<int, int> enemiesRemaining = new Dictionary<int, int>();
    private Dictionary<int, List<Door>> doorsPerFloor = new Dictionary<int, List<Door>>();

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
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.f1Key.wasPressedThisFrame)
        {
            KillAllEnemiesExceptBoss();
        }
    }

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
        
        Debug.Log($"CHEAT: Killed {count} enemies.");
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
