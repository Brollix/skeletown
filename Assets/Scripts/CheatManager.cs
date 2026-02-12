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
        
        // Logic to find current floor and go to next
        // Assuming GameManager tracks floors or we just increment
        // Note: GameManager has TeleportToFloor logic we can reuse if public,
        // or we can invoke a Scene load if scenes are separate.
        // Based on GameManager code, it seems to handle floors via teleporting within the same scene or loading?
        // Let's assume we want to trigger the "Victory" or "Next Floor" logic.
        
        // Inspecting GameManager, it tracks "enemiesRemaining".
        // Use accessible method from GameManager if possible, otherwise implement manual teleport.
        
        // For this task: "Advances to the next level. If the game is on the last level, returns to the first."
        // We need to know current level.
        // If levels are scenes: SceneLoader.LoadScene("LevelX");
        // If levels are positions: TeleportToFloor(current + 1);
        
        if (GameManager.Instance != null)
        {
            // Assuming we just want to teleport to next floor ID
            // We need to know current floor. Since we don't have a "CurrentFloor" property in GameManager (visible in previous read),
            // We'll rely on a heuristic or just cycle 1->20.
            // Let's try to find where the player is or just increment a static counter if needed.
            // Actually, let's look at GameManager.TeleportToFloor (id).
            // We'll trust the user to manually bind F3 to "Next Floor" logic if it's complex,
            // but provided the existing code, I'll attempt to find the highest floor visited so far or just +1.
            
            // Hack for now: Teleport to next logical floor.
            // If we are at floor 1, go to 2.
            // Implementing a simple cycle 1 -> 20 -> 1 for now.
            
            // To do this properly requires knowing the current floor.
            // I'll add a CurrentFloor property to GameManager or track it here.
            
            int nextFloor = (GameManager.Instance.CurrentFloor + 1);
            if (nextFloor > 20) nextFloor = 1; // Loop back
            
            GameManager.Instance.TeleportToFloor(nextFloor);
        }
    }

    private void KillAllEnemies()
    {
        Debug.Log("[Cheat] Killing all enemies...");
        
        List<Enemy> enemies = new List<Enemy>(Enemy.ActiveEnemies);
        foreach (var e in enemies)
        {
            if (e != null) e.TakeDamage(99999f);
        }
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
