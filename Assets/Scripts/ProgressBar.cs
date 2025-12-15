using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image progressBar;      // XP fill bar (bottom blue bar)
    public RectTransform timeLine; // Vertical line

    [Header("Timer Settings")]
    public float maxTimeMinutes = 5f;

    private float elapsedTime = 0f;
    private static ProgressBar instance;


    // Initializes the Singleton instance.
    void Awake()
    {
        if (instance == null)
            instance = this;
    }


    // Resets timeline position and xp bar.
    void Start()
    {
        elapsedTime = 0f;
        UpdateTimeLinePosition(0f);
        UpdateXPBar();
    }


    // Advances timer and updates UI elements.
    void Update()
    {
        // --- Update XP bar ---
        UpdateXPBar();

        // --- Update timeline ---
        elapsedTime += Time.deltaTime;
        float percentTime = Mathf.Clamp01(elapsedTime / (maxTimeMinutes * 60f));
        UpdateTimeLinePosition(percentTime);
    }


    // Updates fill amount based on current XP level progress.
    private void UpdateXPBar()
    {
        if (progressBar == null || PlayerExperience.Instance == null)
            return;

        float current = PlayerExperience.Instance.CurrentXP;
        float required = PlayerExperience.Instance.XPForNextLevel;

        float percent = Mathf.Clamp01(current / required);
        progressBar.fillAmount = percent;
    }


    // Updates the position of the timeline indicator.
    private void UpdateTimeLinePosition(float percent)
    {
        if (timeLine == null || progressBar == null)
            return;

        RectTransform barRect = progressBar.GetComponent<RectTransform>();

        float targetX = percent * barRect.rect.width;
        float pivotOffset = timeLine.pivot.x * timeLine.rect.width;

        Vector2 pos = timeLine.anchoredPosition;
        pos.x = targetX - pivotOffset;
        timeLine.anchoredPosition = pos;
    }


    // Resets timer and visuals to zero.
    public void ResetProgress()
    {
        elapsedTime = 0f;
        UpdateTimeLinePosition(0f);
        UpdateXPBar();
    }

    public static ProgressBar Instance => instance;
}
