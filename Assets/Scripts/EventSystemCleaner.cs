using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Attach this to the EventSystem in your MainMenu or DungeonScene.
/// It will automatically destroy duplicate EventSystems every frame.
/// </summary>
public class EventSystemCleaner : MonoBehaviour
{
    private EventSystem myEventSystem;

    private void Awake()
    {
        myEventSystem = GetComponent<EventSystem>();
    }

    private void Update()
    {
        // Find all EventSystems
        EventSystem[] allSystems = FindObjectsByType<EventSystem>(FindObjectsSortMode.None);
        
        if (allSystems.Length > 1)
        {
            // Destroy all EventSystems except this one
            foreach (var sys in allSystems)
            {
                if (sys != myEventSystem && sys != null)
                {
                    Debug.Log($"[EventSystemCleaner] Destroying duplicate EventSystem from scene: {sys.gameObject.scene.name}");
                    Destroy(sys.gameObject);
                }
            }
        }
    }
}
