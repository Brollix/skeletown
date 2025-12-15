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

    [Header("Upgrade Limits")]
    [SerializeField] private int maxHealthUpgrades = 60;
    [SerializeField] private int maxDamageUpgrades = 50;
    [SerializeField] private int maxSpeedUpgrades = 10;

    public float MaxHealth => baseHealth + (healthUpgrades * healthPerUpgrade);
    public float Damage => baseDamage + (damageUpgrades * damagePerUpgrade);
    public float Speed => baseSpeed + (speedUpgrades * speedPerUpgrade);

    public int GetHealthUpgrades() => healthUpgrades;
    public int GetDamageUpgrades() => damageUpgrades;
    public int GetSpeedUpgrades() => speedUpgrades;

    public bool IsHealthMaxed() => healthUpgrades >= maxHealthUpgrades;
    public bool IsDamageMaxed() => damageUpgrades >= maxDamageUpgrades;
    public bool IsSpeedMaxed() => speedUpgrades >= maxSpeedUpgrades;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadUpgrades();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool UpgradeHealth()
    {
        if (IsHealthMaxed()) return false;

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
        if (IsDamageMaxed()) return false;

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
        if (IsSpeedMaxed()) return false;

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

        SaveUpgrades();
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