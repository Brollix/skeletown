using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    private string _currentSceneName;
    private bool _isLoading = false;

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
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Boot")
        {
            // Initial boot logic
            LoadScene("MainMenu");
        }
    }

    public void LoadScene(string sceneName)
    {
        if (_isLoading) return;
        StartCoroutine(PerformSceneSwitch(sceneName));
    }

    private System.Collections.IEnumerator PerformSceneSwitch(string newSceneName)
    {
        _isLoading = true;

        // 1. Unload the current scene if one is loaded
        if (!string.IsNullOrEmpty(_currentSceneName))
        {
            AsyncOperation unloadOp = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(_currentSceneName);
            while (unloadOp != null && !unloadOp.isDone)
            {
                yield return null;
            }
        }

        // 2. Load the new scene additively
        AsyncOperation loadOp = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(newSceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        while (!loadOp.isDone)
        {
            yield return null;
        }

        // 3. Mark the new scene as active (crucial for instantiation and lighting)
        UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneByName(newSceneName));

        // 4. Cleanup
        _currentSceneName = newSceneName;
        _isLoading = false;
    }
}
