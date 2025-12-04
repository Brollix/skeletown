using UnityEngine;
using UnityEngine.EventSystems;

public class SingletonEventSystem : MonoBehaviour
{
    private void Awake()
    {
        EventSystem[] systems = FindObjectsByType<EventSystem>(FindObjectsSortMode.None);
        if (systems.Length > 1)
        {
            // Keep the one that is already running or the first one found
            // If this one is the duplicate, destroy it
            bool foundOther = false;
            foreach (var sys in systems)
            {
                if (sys.gameObject != gameObject)
                {
                    foundOther = true;
                    break;
                }
            }

            if (foundOther)
            {
                Debug.LogWarning("[SingletonEventSystem] Duplicate EventSystem found. Destroying this one.");
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
