using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // State Tracking
    public Dictionary<int, int> enemiesRemaining = new Dictionary<int, int>();
    private Dictionary<int, List<Door>> doorsPerFloor = new Dictionary<int, List<Door>>();

    private void Awake()
    {
        // Singleton Logic
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        // Prevent duplicate subscriptions by unsubscribing first
        EnemySpawn.OnEnemiesSpawned -= AddEnemies;
        EnemySpawn.OnEnemiesSpawned += AddEnemies;
    }

    private void OnDisable()
    {
        EnemySpawn.OnEnemiesSpawned -= AddEnemies;
    }

    // --- Level State Management ---

    public void ResetLevelState()
    {
        enemiesRemaining.Clear();
        doorsPerFloor.Clear();
        Debug.Log("[GameManager] 🔄 Level State Reset (Enemies and Doors cleared).");
    }

    // --- Enemy Logic ---

    private void AddEnemies(int floor, int amount)
    {
        if (!enemiesRemaining.ContainsKey(floor))
        {
            enemiesRemaining.Add(floor, 0);
        }

        enemiesRemaining[floor] += amount;
        Debug.Log($"[GameManager] 💀 Floor {floor}: Added {amount} enemies. Total: {enemiesRemaining[floor]}");
    }

    public void EnemyDied(int floor)
    {
        if (enemiesRemaining.ContainsKey(floor))
        {
            enemiesRemaining[floor]--;
            Debug.Log($"[GameManager] ⚔️ Enemy died on Floor {floor}. Remaining: {enemiesRemaining[floor]}");

            if (enemiesRemaining[floor] <= 0)
            {
                enemiesRemaining[floor] = 0; // Safety clamp
                OpenDoors(floor);
            }
        }
    }

    // --- Door Logic ---

    public void RegisterDoor(int floor, Door door)
    {
        if (!doorsPerFloor.ContainsKey(floor))
        {
            doorsPerFloor.Add(floor, new List<Door>());
        }

        if (!doorsPerFloor[floor].Contains(door))
        {
            doorsPerFloor[floor].Add(door);
            Debug.Log($"[GameManager] 🚪 Registered Door on Floor {floor}.");
        }
    }

    private void OpenDoors(int floor)
    {
        if (doorsPerFloor.ContainsKey(floor))
        {
            Debug.Log($"[GameManager] 🔓 Opening {doorsPerFloor[floor].Count} doors on Floor {floor}!");
            foreach (var door in doorsPerFloor[floor])
            {
                if (door != null) door.SetOpen(true);
            }
        }
        else
        {
            Debug.LogWarning($"[GameManager] Floor {floor} cleared, but no doors registered!");
        }
    }

    // --- Cheats / Debug ---

    public void KillAllEnemies()
    {
        Debug.Log("[GameManager] 🕵️ CHEAT: Killing all enemies...");
        var keys = new List<int>(enemiesRemaining.Keys);
        foreach (var floor in keys)
        {
            enemiesRemaining[floor] = 0;
            OpenDoors(floor);
        }
    }
}
