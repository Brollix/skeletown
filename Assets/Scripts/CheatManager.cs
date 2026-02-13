using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class CheatManager : MonoBehaviour
{
    public static CheatManager Instance;

    [Header("References")]
    [SerializeField] private CheatUI cheatUI;

    [Header("Cheat Settings")]
    public float SpeedMultiplier = 3f;

    // Cheat States
    public bool IsGodMode { get; private set; } = false;
    public bool IsSuperSpeed { get; private set; } = false;

    private void Awake()
    {
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

    private void Start()
    {
        // Try to find UI if not assigned
        if (cheatUI == null)
            cheatUI = GetComponentInChildren<CheatUI>();
    }

    private void Update()
    {
        if (Keyboard.current == null) return;

        // F1: Toggle UI
        if (Keyboard.current.f1Key.wasPressedThisFrame)
        {
            if (cheatUI != null) cheatUI.TogglePanel();
        }

        // F2: God Mode
        if (Keyboard.current.f2Key.wasPressedThisFrame)
        {
            ToggleGodMode();
        }

        // F3: Next Level
        if (Keyboard.current.f3Key.wasPressedThisFrame)
        {
            GoToNextLevel();
        }

        // F4: Kill All Enemies
        if (Keyboard.current.f4Key.wasPressedThisFrame)
        {
            KillAllEnemies();
        }

        // F5: Speed
        if (Keyboard.current.f5Key.wasPressedThisFrame)
        {
            ToggleSpeed();
        }

        // F6: Give XP / Level Up
        if (Keyboard.current.f6Key.wasPressedThisFrame)
        {
            GiveXPCheat();
        }
    }

    private void ToggleGodMode()
    {
        IsGodMode = !IsGodMode;
        Debug.Log($"[Cheat] God Mode: {IsGodMode}");
        UpdateUI();
    }

    private void ToggleSpeed()
    {
        IsSuperSpeed = !IsSuperSpeed;
        Debug.Log($"[Cheat] Super Speed: {IsSuperSpeed}");
        UpdateUI();
    }

    private void GoToNextLevel()
    {
        Debug.Log("[Cheat] Advancing to Next Level...");
        
        if (GameManager.Instance != null)
        {
            // Use our smart detection to find where we actually are
            int currentFloor = GetCurrentFloor();
            int nextFloor = currentFloor + 1;
            
            if (nextFloor > 20) nextFloor = 1; // Loop back
            
            Debug.Log($"[Cheat] Teleporting from {currentFloor} to {nextFloor}");
            GameManager.Instance.TeleportToFloor(nextFloor);
        }
    }

    private void KillAllEnemies()
    {
        int currentFloor = GetCurrentFloor();
        Debug.Log($"[Cheat] Killing all enemies on Floor {currentFloor}...");
        
        List<Enemy> enemies = new List<Enemy>(Enemy.ActiveEnemies);
        foreach (var e in enemies)
        {
            if (e != null && e.floorNumber == currentFloor)
            {
                e.TakeDamage(99999f);
            }
        }
    }

    private int GetCurrentFloor()
    {
        if (Player.Instance == null) return 1;

        Vector2 playerPos = Player.Instance.transform.position;
        float minDistance = float.MaxValue;
        int closestFloor = 1;
        bool foundAny = false;

        // Check Doors (they have public floorNumber)
        Door[] doors = FindObjectsOfType<Door>();
        foreach (var door in doors)
        {
            float dist = Vector2.Distance(playerPos, door.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closestFloor = door.floorNumber;
                foundAny = true;
            }
        }

        // Check EnemySpawns (they are inside the floor)
        EnemySpawn[] spawners = FindObjectsOfType<EnemySpawn>();
        foreach (var spawner in spawners)
        {
            float dist = Vector2.Distance(playerPos, spawner.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                // EnemySpawn floorNumber is private, so grab from parent FloorID
                var floorsID = spawner.GetComponentInParent<FloorID>();
                if (floorsID != null)
                {
                    closestFloor = floorsID.floorNumber;
                    foundAny = true;
                }
            }
        }

        if (foundAny)
        {
            Debug.Log($"[Cheat] Detected closest floor: {closestFloor} (Dist: {minDistance:F1})");
            return closestFloor;
        }

        return 1; // Default
    }

    private void GiveXPCheat()
    {
        Debug.Log("[Cheat] Giving XP...");
        if (PlayerExperience.Instance != null)
        {
            PlayerExperience.Instance.AddXP(100f); // Arbitrary amount
        }
    }

    private void UpdateUI()
    {
        if (cheatUI != null)
        {
            cheatUI.UpdateStatus(IsGodMode, IsSuperSpeed);
        }
    }
}
