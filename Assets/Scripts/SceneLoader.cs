using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

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
        // Only load Main Menu if we are in the Boot scene
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Boot")
        {
            StartCoroutine(LoadMainMenuWithDelay());
        }
    }

    private System.Collections.IEnumerator LoadMainMenuWithDelay()
    {
        // Create a temporary camera to avoid "No Cameras Rendering" flash
        if (Camera.main == null)
        {
            GameObject camObj = new GameObject("TemporaryBootCamera");
            Camera cam = camObj.AddComponent<Camera>();
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = Color.black;
        }

        // Wait one frame to let the black screen render
        yield return null;

        LoadScene("MainMenu");
    }

    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
