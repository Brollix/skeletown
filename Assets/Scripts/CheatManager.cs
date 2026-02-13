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
        if (cheatUI == null)
            cheatUI = GetComponentInChildren<CheatUI>();
    }

    private void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.f1Key.wasPressedThisFrame)
        {
            if (cheatUI != null) cheatUI.TogglePanel();
        }

        if (Keyboard.current.f2Key.wasPressedThisFrame)
        {
            ToggleGodMode();
        }

        if (Keyboard.current.f3Key.wasPressedThisFrame)
        {
            GoToNextLevel();
        }

        if (Keyboard.current.f4Key.wasPressedThisFrame)
        {
            KillAllEnemies();
        }

        if (Keyboard.current.f5Key.wasPressedThisFrame)
        {
            ToggleSpeed();
        }

        if (Keyboard.current.f6Key.wasPressedThisFrame)
        {
            GiveXPCheat();
        }
    }

    private void ToggleGodMode()
    {
        IsGodMode = !IsGodMode;
        UpdateUI();
    }

    private void ToggleSpeed()
    {
        IsSuperSpeed = !IsSuperSpeed;
        UpdateUI();
    }

    private void GoToNextLevel()
    {
        if (GameManager.Instance != null)
        {
            int currentFloor = GetCurrentFloor();
            int nextFloor = currentFloor + 1;
            
            if (nextFloor > 20) nextFloor = 1;
            
            GameManager.Instance.TeleportToFloor(nextFloor);
        }
    }

    private void KillAllEnemies()
    {
        int currentFloor = GetCurrentFloor();
        
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

        EnemySpawn[] spawners = FindObjectsOfType<EnemySpawn>();
        foreach (var spawner in spawners)
        {
            float dist = Vector2.Distance(playerPos, spawner.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
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
            return closestFloor;
        }

        return 1;
    }

    private void GiveXPCheat()
    {
        if (PlayerExperience.Instance != null)
        {
            PlayerExperience.Instance.AddXP(100f);
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
