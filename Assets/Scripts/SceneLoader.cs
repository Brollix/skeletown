using System.Collections;
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

        yield return UnloadCurrentSceneIfAnyLoaded(_currentSceneName);

        AsyncOperation loadOp = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(newSceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        while (!loadOp.isDone)
        {
            yield return null;
        }

        UnityEngine.SceneManagement.Scene activeScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(newSceneName);
        if (activeScene.IsValid())
        {
            UnityEngine.SceneManagement.SceneManager.SetActiveScene(activeScene);
        }

        _currentSceneName = newSceneName;
        _isLoading = false;

        IEnumerator UnloadCurrentSceneIfAnyLoaded(string currentSceneName)
        {
            if (!string.IsNullOrEmpty(currentSceneName))
            {
                AsyncOperation unloadOp = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currentSceneName);
                while (unloadOp != null && !unloadOp.isDone)
                {
                    yield return null;
                }
            }
        }
    }
}
