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
    [SerializeField] private int maxLevel = 121;
    [SerializeField] private float currentXP = 0f;
    [SerializeField] private int skillPoints = 0;

    public int CurrentLevel => currentLevel;
    public float CurrentXP => currentXP;
    public int SkillPoints => skillPoints;
    public float XPForNextLevel => CalculateXPForLevel(currentLevel + 1);

    public event Action<int> OnLevelUp;
    public event Action<float> OnXPChanged;

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

    public void AddXP(float amount)
    {
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

    public void AddSkillPoints(int amount)
    {
        skillPoints += amount;
        SaveProgress();
    }

    private void LevelUp()
    {
        currentLevel++;
        skillPoints += 1;
        OnLevelUp?.Invoke(currentLevel);
    }

    private float CalculateXPForLevel(int level)
    {
        float xp = baseXPPerLevel * Mathf.Pow(xpMultiplier, level - 1);
        return Mathf.Clamp(xp, 0, 250f);
    }

    public void LoadProgress()
    {
        currentLevel = PlayerPrefs.GetInt(GameConstants.PREF_PLAYER_LEVEL, 1);
        currentXP = PlayerPrefs.GetFloat(GameConstants.PREF_PLAYER_XP, 0f);
        skillPoints = PlayerPrefs.GetInt(GameConstants.PREF_SKILL_POINTS, 0);
    }

    public void SaveProgress()
    {
        PlayerPrefs.SetInt(GameConstants.PREF_PLAYER_LEVEL, currentLevel);
        PlayerPrefs.SetFloat(GameConstants.PREF_PLAYER_XP, currentXP);
        PlayerPrefs.SetInt(GameConstants.PREF_SKILL_POINTS, skillPoints);
        PlayerPrefs.Save();
    }

    public void RefundAllSkillPoints(int amount)
    {
        skillPoints += amount;
        SaveProgress();
    }
}