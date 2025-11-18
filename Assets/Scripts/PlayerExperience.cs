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
        skillPoints += 1; // 1 skill point per level
        OnLevelUp?.Invoke(currentLevel);
    }

    private float CalculateXPForLevel(int level)
    {
        return baseXPPerLevel * Mathf.Pow(xpMultiplier, level - 1);
    }

    private void LoadProgress()
    {
        // Always start fresh - ignore saved progress for now
        currentLevel = 1;
        currentXP = 0f;
        skillPoints = 0;

        Debug.Log("🎮 Player progress reset - Level: 1, XP: 0, Skill Points: 0");
        Debug.Log("⬆️ Player upgrades preserved from previous sessions");
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetInt("PlayerLevel", currentLevel);
        PlayerPrefs.SetFloat("PlayerXP", currentXP);
        PlayerPrefs.SetInt("SkillPoints", skillPoints);
        PlayerPrefs.Save();
    }
}