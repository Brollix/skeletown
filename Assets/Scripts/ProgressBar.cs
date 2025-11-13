using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("Gray background of the bar")]
    public Image background;   // Gray background
    
    [Tooltip("Red progress bar (must be Filled type)")]
    public Image progressBar;  // Red bar (Filled type)
    
    [Tooltip("Vertical line showing remaining time")]
    public RectTransform timeLine; // Vertical line

    [Header("Configuration")]
    [Tooltip("Max time in minutes (5 minutes = 300 seconds)")]
    public float maxTimeMinutes = 5f;
    
    [Tooltip("Total enemies to eliminate in this level")]
    public int totalEnemiesInLevel = 10;

    [Header("Current Values (Runtime)")]
    [Tooltip("Enemies killed")]
    [SerializeField] private int enemiesKilled = 0;
    
    [Tooltip("Elapsed time in seconds")]
    [SerializeField] private float elapsedTime = 0f;

    [Header("State")]
    [Tooltip("If timer is active")]
    public bool isTimerActive = true;

    private static ProgressBar instance;

    void Awake()
    {
        // Singleton pattern for easy access from other scripts
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.LogWarning("Multiple ProgressBar instances found. Only one should exist.");
        }
    }

    void Start()
    {
        // Validate all elements are assigned
        if (background == null)
            Debug.LogWarning("ProgressBar: Background not assigned in Inspector");
        if (progressBar == null)
            Debug.LogWarning("ProgressBar: ProgressBar not assigned in Inspector");
        if (timeLine == null)
            Debug.LogWarning("ProgressBar: TimeLine not assigned in Inspector");

        // If total enemies not set, try calculating automatically
        if (totalEnemiesInLevel <= 0)
        {
            CalculateTotalEnemiesFromSpawners();
        }

        // Reset values
        Reset();
    }

    /// <summary>
    /// Automatically calculates total enemies by summing all EnemySpawn in scene
    /// </summary>
    public void CalculateTotalEnemiesFromSpawners()
    {
        EnemySpawn[] spawners = FindObjectsByType<EnemySpawn>(FindObjectsSortMode.None);
        int total = 0;
        
        foreach (EnemySpawn spawner in spawners)
        {
            total += spawner.numberToSpawn;
        }

        if (total > 0)
        {
            totalEnemiesInLevel = total;
            Debug.Log($"ProgressBar: Total enemies calculated automatically: {total}");
        }
        else
        {
            Debug.LogWarning("ProgressBar: No enemy spawners found. Set 'Total Enemies In Level' manually.");
        }
    }

    void Update()
    {
        // Advance time automatically (5 minutes left to right)
        if (isTimerActive)
        {
            elapsedTime += Time.deltaTime;
            elapsedTime = Mathf.Clamp(elapsedTime, 0f, maxTimeMinutes * 60f);
        }

        // Update progress bar (red) based on enemies killed
        if (progressBar != null)
        {
            float percentProgress = 0f;
            if (totalEnemiesInLevel > 0)
            {
                percentProgress = Mathf.Clamp01((float)enemiesKilled / (float)totalEnemiesInLevel);
            }
            progressBar.fillAmount = percentProgress;
        }

        // Move vertical time line (left to right in 5 minutes)
        if (timeLine != null && progressBar != null)
        {
            float percentTime = Mathf.Clamp01(elapsedTime / (maxTimeMinutes * 60f));
            RectTransform progressRect = progressBar.GetComponent<RectTransform>();
            if (progressRect != null)
            {
                // Calculate position from left edge (0) to right edge (width)
                float targetX = percentTime * progressRect.rect.width;
                
                // Adjust for timeLine's pivot: if pivot is at center (0.5), subtract half width
                // This ensures the line starts at the left edge when percentTime = 0
                float pivotOffset = timeLine.pivot.x * timeLine.rect.width;
                float x = targetX - pivotOffset;
                
                Vector2 pos = timeLine.anchoredPosition;
                pos.x = x;
                timeLine.anchoredPosition = pos;
            }
        }
    }

    /// <summary>
    /// Registers enemy killed (called from Enemy.cs)
    /// </summary>
    public void OnEnemyKilled()
    {
        enemiesKilled++;
        enemiesKilled = Mathf.Clamp(enemiesKilled, 0, totalEnemiesInLevel);
    }

    /// <summary>
    /// Sets total enemies in level
    /// </summary>
    public void SetTotalEnemies(int total)
    {
        totalEnemiesInLevel = Mathf.Max(1, total);
    }

    /// <summary>
    /// Sets elapsed time in seconds
    /// </summary>
    public void SetElapsedTime(float seconds)
    {
        elapsedTime = Mathf.Clamp(seconds, 0f, maxTimeMinutes * 60f);
    }

    /// <summary>
    /// Gets remaining time in seconds
    /// </summary>
    public float GetRemainingTime()
    {
        return Mathf.Max(0f, (maxTimeMinutes * 60f) - elapsedTime);
    }

    /// <summary>
    /// Gets elapsed time in seconds
    /// </summary>
    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    /// <summary>
    /// Gets number of enemies killed
    /// </summary>
    public int GetEnemiesKilled()
    {
        return enemiesKilled;
    }

    /// <summary>
    /// Resets bar (progress and time to 0)
    /// </summary>
    public void Reset()
    {
        enemiesKilled = 0;
        elapsedTime = 0f;
        
        // Reset time line position to left edge
        if (timeLine != null && progressBar != null)
        {
            RectTransform progressRect = progressBar.GetComponent<RectTransform>();
            if (progressRect != null)
            {
                // Position at left edge (0), adjusting for timeLine's pivot
                float pivotOffset = timeLine.pivot.x * timeLine.rect.width;
                float x = 0f - pivotOffset;
                
                Vector2 pos = timeLine.anchoredPosition;
                pos.x = x;
                timeLine.anchoredPosition = pos;
            }
        }
    }

    /// <summary>
    /// Pauses or resumes timer
    /// </summary>
    public void SetTimerActive(bool active)
    {
        isTimerActive = active;
    }

    /// <summary>
    /// Gets static ProgressBar instance (for access from other scripts)
    /// </summary>
    public static ProgressBar GetInstance()
    {
        return instance;
    }
}

