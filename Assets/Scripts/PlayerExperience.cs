using UnityEngine;
using System;

public class PlayerExperience : MonoBehaviour
{
    public static PlayerExperience Instance;

    [Header("Experience Settings")]
    [SerializeField] private float baseXPPerLevel = 100f;
    [SerializeField] private float xpMultiplier = 1.2f;

    [Header("Current Stats")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int maxLevel = 121; // Cap level to limit total skill points
    [SerializeField] private float currentXP = 0f;
    [SerializeField] private int skillPoints = 0;

    public int CurrentLevel => currentLevel;
    public float CurrentXP => currentXP;
    public int SkillPoints => skillPoints;
    public float XPForNextLevel => CalculateXPForLevel(currentLevel + 1);

    public event Action<int> OnLevelUp;
    public event Action<float> OnXPChanged;


    // Initializes Singleton and loads persistent experience data.
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadProgress();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Grants experience points and checks for level ups.
    public void AddXP(float amount)
    {
        // If max level reached, we can optionally stop gaining XP or just stop leveling up.
        // User asked for "limit for total XP points", implying hitting a ceiling.
        if (currentLevel >= maxLevel) return;

        currentXP += amount;
        OnXPChanged?.Invoke(currentXP);

        while (currentXP >= XPForNextLevel)
        {
            currentXP -= XPForNextLevel;
            LevelUp();
        }

        SaveProgress();
    }


    // Spends a skill point if available.
    public bool ConsumeSkillPoint()
    {
        if (skillPoints > 0)
        {
            skillPoints--;
            SaveProgress();
            return true;
        }
        return false;
    }


    // Grants skill points directly (used for cheats or resets).
    public void AddSkillPoints(int amount)
    {
        skillPoints += amount;
        SaveProgress();
    }


    // Increments player level and grants a skill point.
    private void LevelUp()
    {
        currentLevel++;
        skillPoints += 1; // 1 skill point per level
        OnLevelUp?.Invoke(currentLevel);
    }


    // Calculates the XP required to reach the next level.
    private float CalculateXPForLevel(int level)
    {
        float xp = baseXPPerLevel * Mathf.Pow(xpMultiplier, level - 1);
        return Mathf.Clamp(xp, 0, 250f); // NEVER require more than 250 XP
    }



    // Loads experience stats from PlayerPrefs.
    public void LoadProgress()
    {
        currentLevel = PlayerPrefs.GetInt("PlayerLevel", 1);
        currentXP = PlayerPrefs.GetFloat("PlayerXP", 0f);
        skillPoints = PlayerPrefs.GetInt("SkillPoints", 0);

        Debug.Log($"Loaded progress - Level: {currentLevel}, XP: {currentXP}, Skill Points: {skillPoints}");
    }



    // Saves experience stats to PlayerPrefs.
    public void SaveProgress()
    {
        PlayerPrefs.SetInt("PlayerLevel", currentLevel);
        PlayerPrefs.SetFloat("PlayerXP", currentXP);
        PlayerPrefs.SetInt("SkillPoints", skillPoints);
        PlayerPrefs.Save();
    }


    // Helper to return skill points during a reset.
    public void RefundAllSkillPoints(int amount)
    {
        skillPoints += amount;
        SaveProgress();
    }

}