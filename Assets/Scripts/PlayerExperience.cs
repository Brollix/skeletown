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
        if (Instance != null && Instance != this)
        {
             // If the existing instance is in the MainMenu or Boot scene, and I am NOT,
            // then I am the "real" manager for the game world. Destroy the old one.
            string oldScene = Instance.gameObject.scene.name;
            string myScene = gameObject.scene.name;

            if ((oldScene == "MainMenu" || oldScene == "Boot") && myScene != oldScene)
            {
                Debug.Log($"[PlayerExperience] Overwriting Singleton. Old: {oldScene}, New: {myScene}");
                Destroy(Instance.gameObject);
                Instance = this;
                LoadProgress();
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }
        else
        {
            Instance = this;
            LoadProgress();
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
        float xp = baseXPPerLevel * Mathf.Pow(xpMultiplier, level - 1);
        return Mathf.Clamp(xp, 0, 250f); // NEVER require more than 250 XP
    }


    public void LoadProgress()
    {
        currentLevel = PlayerPrefs.GetInt("PlayerLevel", 1);
        currentXP = PlayerPrefs.GetFloat("PlayerXP", 0f);
        skillPoints = PlayerPrefs.GetInt("SkillPoints", 0);

        Debug.Log($"Loaded progress - Level: {currentLevel}, XP: {currentXP}, Skill Points: {skillPoints}");
    }


    public void SaveProgress()
    {
        PlayerPrefs.SetInt("PlayerLevel", currentLevel);
        PlayerPrefs.SetFloat("PlayerXP", currentXP);
        PlayerPrefs.SetInt("SkillPoints", skillPoints);
        PlayerPrefs.Save();
    }

    public void RefundAllSkillPoints(int amount)
    {
        skillPoints += amount;
        SaveProgress();
    }

}