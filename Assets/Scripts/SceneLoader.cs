using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [Header("Scene Names")]
    public string mainMenuScene = "MainMenu";
    public string worldScene = "DungeonScene";
    public string creditsScene = "Credits";

    [Header("Audio")]
    [SerializeField] private AudioClip dungeonMusic;

    // Tracks the currently active scene (MainMenu, DungeonScene, etc.)
    private string currentActiveScene;

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
        // On startup (Boot), current scene is Boot
        currentActiveScene = SceneManager.GetActiveScene().name;

        // Automatically transition to MainMenu if we start in Boot
        if (currentActiveScene == "Boot" || currentActiveScene == "Initialization")
        {
            LoadMainMenu();
        }
    }

    // Public API for UI Buttons
    public void LoadMainMenu()
    {
        StartCoroutine(LoadSceneRoutine(mainMenuScene));
    }

    public void LoadWorld()
    {
        StartCoroutine(LoadSceneRoutine(worldScene));
    }

    public void LoadCredits()
    {
        StartCoroutine(LoadSceneRoutine(creditsScene));
    }

    private IEnumerator LoadSceneRoutine(string nextSceneName)
    {
        Debug.Log($"[SceneLoader] 🏁 Starting transition from '{currentActiveScene}' to '{nextSceneName}'");

        // 1. Capture the scene we are leaving so we can unload it later
        string sceneToUnload = currentActiveScene;

        // 2. Pre-Load Logic (Music, Game State Reset)
        HandlePreLoadLogic(nextSceneName);

        // 3. Load the new scene Additively
        // Check if it's already loaded to avoid duplicates (safety check)
        Scene existingScene = SceneManager.GetSceneByName(nextSceneName);
        if (!existingScene.isLoaded)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
            while (!asyncLoad.isDone) yield return null;
        }

        // 4. Set Active Scene
        Scene newScene = SceneManager.GetSceneByName(nextSceneName);
        if (newScene.IsValid())
        {
            SceneManager.SetActiveScene(newScene);
            currentActiveScene = nextSceneName; // Update tracker immediately
            Debug.Log($"[SceneLoader] ✅ Active scene set to: {currentActiveScene}");
        }

        // 5. Cleanup (Unload old scene, remove duplicate EventSystems/Listeners)
        yield return StartCoroutine(UnloadSceneRoutine(sceneToUnload, nextSceneName));
        
        CleanupDuplicates(nextSceneName);

        Debug.Log($"[SceneLoader] 🎉 Transition Complete. Loaded: {SceneManager.sceneCount} scenes.");
    }

    private void HandlePreLoadLogic(string nextSceneName)
    {
        // Music Logic
        if (AudioManager.Instance != null)
        {
            if (nextSceneName == worldScene)
                AudioManager.Instance.PlayMusic(dungeonMusic);
            else if (nextSceneName == mainMenuScene)
                AudioManager.Instance.StopMusic();
        }

        // GameManager Reset Logic
        // If we are going to the Dungeon, we MUST reset the game state (enemies, doors)
        if (nextSceneName == worldScene && GameManager.Instance != null)
        {
            GameManager.Instance.ResetLevelState();
        }
    }

    private IEnumerator UnloadSceneRoutine(string sceneName, string sceneToKeep)
    {
        // Don't unload if it's the same scene we just loaded (shouldn't happen, but safety)
        if (sceneName == sceneToKeep) yield break;
        
        // Don't unload if string is empty
        if (string.IsNullOrEmpty(sceneName)) yield break;

        Debug.Log($"[SceneLoader] 🗑️ Attempting to unload: {sceneName}");
        Scene scene = SceneManager.GetSceneByName(sceneName);
        
        if (scene.IsValid() && scene.isLoaded)
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);
            while (!asyncUnload.isDone) yield return null;
            Debug.Log($"[SceneLoader] ✅ Unloaded: {sceneName}");
        }
        else
        {
            Debug.LogWarning($"[SceneLoader] Could not unload '{sceneName}' - it might not be loaded.");
        }
    }

    private void CleanupDuplicates(string activeSceneName)
    {
        // 1. Cleanup EventSystems (Keep only the one in the active scene)
        EventSystem[] systems = FindObjectsByType<EventSystem>(FindObjectsSortMode.None);
        if (systems.Length > 1)
        {
            bool foundActiveSceneSystem = false;
            foreach (var sys in systems)
            {
                // Keep the FIRST one we find in the active scene
                if (sys.gameObject.scene.name == activeSceneName && !foundActiveSceneSystem)
                {
                    foundActiveSceneSystem = true;
                    continue; // Keep this one
                }
                
                // Destroy all others
                Destroy(sys.gameObject);
            }
        }

        // 2. Cleanup AudioListeners (Keep only one)
        AudioListener[] listeners = FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
        if (listeners.Length > 1)
        {
            bool foundActiveSceneListener = false;
            foreach (var listener in listeners)
            {
                // Keep the FIRST one in the active scene
                if (listener.gameObject.scene.name == activeSceneName && !foundActiveSceneListener)
                {
                    foundActiveSceneListener = true;
                    continue;
                }
                
                // Destroy others
                Destroy(listener);
            }
        }
        
        // 3. Special Boot Camera Cleanup
        if (activeSceneName != "Boot")
        {
            Camera[] cameras = FindObjectsByType<Camera>(FindObjectsSortMode.None);
            foreach (var cam in cameras)
            {
                if (cam.gameObject.scene.name == "Boot") Destroy(cam.gameObject);
            }
        }
    }
}
