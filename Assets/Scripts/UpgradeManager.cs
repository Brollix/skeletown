using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [Header("Base Stats")]
    [SerializeField] private float baseHealth = 10f;
    [SerializeField] private float baseDamage = 1f;
    [SerializeField] private float baseSpeed = 5f;

    [Header("Upgrade Multipliers")]
    [SerializeField] private float healthPerUpgrade = 2f;
    [SerializeField] private float damagePerUpgrade = 0.5f;
    [SerializeField] private float speedPerUpgrade = 0.5f;

    [Header("Current Upgrades")]
    [SerializeField] private int healthUpgrades = 0;
    [SerializeField] private int damageUpgrades = 0;
    [SerializeField] private int speedUpgrades = 0;

    public float MaxHealth => baseHealth + (healthUpgrades * healthPerUpgrade);
    public float Damage => baseDamage + (damageUpgrades * damagePerUpgrade);
    public float Speed => baseSpeed + (speedUpgrades * speedPerUpgrade);

    public int GetHealthUpgrades() => healthUpgrades;
    public int GetDamageUpgrades() => damageUpgrades;
    public int GetSpeedUpgrades() => speedUpgrades;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadUpgrades();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool UpgradeHealth()
    {
        if (PlayerExperience.Instance.ConsumeSkillPoint())
        {
            healthUpgrades++;
            SaveUpgrades();
            return true;
        }
        return false;
    }

    public bool UpgradeDamage()
    {
        if (PlayerExperience.Instance.ConsumeSkillPoint())
        {
            damageUpgrades++;
            SaveUpgrades();
            return true;
        }
        return false;
    }

    public bool UpgradeSpeed()
    {
        if (PlayerExperience.Instance.ConsumeSkillPoint())
        {
            speedUpgrades++;
            SaveUpgrades();
            return true;
        }
        return false;
    }

    public void ResetAllUpgrades()
    {
        healthUpgrades = 0;
        damageUpgrades = 0;
        speedUpgrades = 0;
    }

    public void LoadUpgrades()
    {
        healthUpgrades = PlayerPrefs.GetInt("HealthUpgrades", 0);
        damageUpgrades = PlayerPrefs.GetInt("DamageUpgrades", 0);
        speedUpgrades = PlayerPrefs.GetInt("SpeedUpgrades", 0);

        Debug.Log($"⬆️ Upgrades loaded - Health: {healthUpgrades}, Damage: {damageUpgrades}, Speed: {speedUpgrades}");
    }

    public void SaveUpgrades()
    {
        PlayerPrefs.SetInt("HealthUpgrades", healthUpgrades);
        PlayerPrefs.SetInt("DamageUpgrades", damageUpgrades);
        PlayerPrefs.SetInt("SpeedUpgrades", speedUpgrades);
        PlayerPrefs.Save();
    }
}