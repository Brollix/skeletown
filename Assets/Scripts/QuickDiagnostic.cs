using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Add this to Boot scene Managers object.
/// It will print a simple diagnostic every 2 seconds.
/// </summary>
public class QuickDiagnostic : MonoBehaviour
{
    private void Start()
    {
        InvokeRepeating("PrintStatus", 2f, 2f);
    }

    private void PrintStatus()
    {
        Debug.Log("========================================");
        Debug.Log($"SCENES LOADED: {SceneManager.sceneCount}");
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Debug.Log($"  {i}. {SceneManager.GetSceneAt(i).name}");
        }
        
        Debug.Log($"SceneLoader exists: {SceneLoader.Instance != null}");
        Debug.Log($"GameManager exists: {GameManager.Instance != null}");
        
        if (GameManager.Instance != null && GameManager.Instance.enemiesRemaining != null)
        {
            int total = 0;
            foreach (var kvp in GameManager.Instance.enemiesRemaining)
            {
                total += kvp.Value;
                Debug.Log($"  Floor {kvp.Key}: {kvp.Value} enemies");
            }
            Debug.Log($"TOTAL ENEMIES: {total}");
        }
        
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        Debug.Log($"ACTUAL ENEMY OBJECTS: {enemies.Length}");
        Debug.Log("========================================");
    }
}
