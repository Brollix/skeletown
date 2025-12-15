using UnityEngine;
using System.Collections.Generic;

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

    public void ResetGame()
    {
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
    // DEBUG: PRESS K TO KILL ALL ENEMIES
    // ------------------------------------------------------
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    KillAllEnemiesOnAllFloors();
        //}
        //
        // if (Input.GetKeyDown(KeyCode.L))
        // {
        //     PrintFloorStates();
        // }
    }

    // ------------------------------------------------------
    // KILL ALL ENEMIES
    // ------------------------------------------------------
    // private void KillAllEnemiesOnAllFloors()
    // {
    //     Enemy[] enemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);

    //     int totalKilled = 0;

    //     foreach (Enemy e in enemies)
    //     {
    //         if (e != null)
    //         {
    //             e.TakeDamage(999999f);
    //             totalKilled++;
    //         }
    //     }

    //     Debug.Log("DEBUG: Killed all enemies. Total killed: " + totalKilled);
    // }

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
