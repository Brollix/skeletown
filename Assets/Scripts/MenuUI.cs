using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject upgradesPanel;
    public GameObject creditsPanel;

    private void Start()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        upgradesPanel.SetActive(false);
        creditsPanel.SetActive(false);

        // Enforce UI Scaling
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            UnityEngine.UI.CanvasScaler scaler = canvas.GetComponent<UnityEngine.UI.CanvasScaler>();
            if (scaler == null)
            {
                scaler = canvas.gameObject.AddComponent<UnityEngine.UI.CanvasScaler>();
            }

            scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
        }

        // Force Canvas to Overlay to ensure visibility
        if (canvas != null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }
    }

    public void StartGame()
    {
        Debug.Log("[MenuUI] StartGame() called!");
        
        if (SceneLoader.Instance != null)
        {
            Debug.Log($"[MenuUI] SceneLoader exists. worldScene = '{SceneLoader.Instance.worldScene}'");
            SceneLoader.Instance.LoadWorld();
            Debug.Log("[MenuUI] LoadWorld() called!");
        }
        else
        {
            Debug.LogError("[MenuUI] SceneLoader.Instance is NULL!");
            // Fallback for direct play from MainMenu
            SceneManager.LoadScene("DungeonScene");
        }
    }
    

    public void QuitGame()
    {
        Debug.Log("Quit pressed");
        Application.Quit();
    }

    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void OpenUpgrades()
    {
        mainMenuPanel.SetActive(false);
        upgradesPanel.SetActive(true);
    }

    public void CloseUpgrades()
    {
        upgradesPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void OpenCredits()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
