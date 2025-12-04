using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

/// <summary>
/// Add this to an empty GameObject in your DungeonScene to debug the enemy count issue.
/// It will log detailed information every frame.
/// </summary>
public class DebugEnemyCount : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.pKey.wasPressedThisFrame) // Press P to print debug info
        {
            PrintDebugInfo();
        }
    }

    private void PrintDebugInfo()
    {
        Debug.Log("========== DEBUG INFO ==========");
        
        // Check how many scenes are loaded
        Debug.Log($"Total scenes loaded: {SceneManager.sceneCount}");
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            Debug.Log($"  Scene {i}: {scene.name} (Active: {scene == SceneManager.GetActiveScene()})");
        }
        
        // Check GameManager state
        if (GameManager.Instance != null)
        {
            Debug.Log($"GameManager exists on: {GameManager.Instance.gameObject.scene.name}");
            
            // Check enemy count
            if (GameManager.Instance.enemiesRemaining != null)
            {
                int totalEnemies = 0;
                foreach (var kvp in GameManager.Instance.enemiesRemaining)
                {
                    Debug.Log($"  Floor {kvp.Key}: {kvp.Value} enemies");
                    totalEnemies += kvp.Value;
                }
                Debug.Log($"Total enemies in GameManager: {totalEnemies}");
            }
        }
        else
        {
            Debug.LogError("GameManager.Instance is NULL!");
        }
        
        // Count actual enemies in all scenes
        Enemy[] allEnemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        Debug.Log($"Actual Enemy objects in scenes: {allEnemies.Length}");
        foreach (var enemy in allEnemies)
        {
            Debug.Log($"  Enemy in scene: {enemy.gameObject.scene.name}, Floor: {enemy.floorNumber}");
        }
        
        Debug.Log("================================");
    }
}
