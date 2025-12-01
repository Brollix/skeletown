using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance;

    [Header("Scene Settings")]
    [SerializeField] private string startSceneName = "DungeonScene";

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
            return;
        }
    }

    public void StartGame()
    {
        LoadScene(startSceneName);
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Application.Quit called");
    }

    public void OnFinalBossDefeated()
    {
        Time.timeScale = 0f;
        ResetAllPlayerProgress();
    }

    private void ResetAllPlayerProgress()
    {
        if (UpgradeManager.Instance == null || PlayerExperience.Instance == null)
        {
            Debug.LogError("ResetAllPlayerProgress: Required managers missing");
            return;
        }

        int totalUsedPoints = UpgradeManager.Instance.GetHealthUpgrades()
                           + UpgradeManager.Instance.GetDamageUpgrades()
                           + UpgradeManager.Instance.GetSpeedUpgrades();

        if (totalUsedPoints > 0)
            PlayerExperience.Instance.AddSkillPoints(totalUsedPoints);

        UpgradeManager.Instance.ResetAllUpgrades();
        UpgradeManager.Instance.SaveUpgrades();

        PlayerExperience.Instance.ResetToLevel0();
        PlayerExperience.Instance.SaveProgress();
    }
}
