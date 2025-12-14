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
            LoadScene("MainMenu");
        }
    }

    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
