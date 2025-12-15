using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music Clips")]
    [SerializeField] private AudioClip dungeonMusic;

    [System.Serializable]
    public class SoundConfig
    {
        public AudioClip clip;
        [Range(0f, 2f)] public float minPitch = 0.9f;
        [Range(0f, 2f)] public float maxPitch = 1.1f;
    }

    [Header("UI Sounds")]
    [SerializeField] private SoundConfig clickSound;
    [SerializeField] private SoundConfig hoverSound;

    [Header("Gameplay Sounds")]
    [SerializeField] private SoundConfig shootSound;
    [SerializeField] private SoundConfig playerHitSound;
    [SerializeField] private SoundConfig enemyHitSound;
    [SerializeField] private SoundConfig playerDeathSound;

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
        }
    }

    private void Start()
    {
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChanged;
        
        // Initial check
        PlayMusicForScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        // Subscribe to Gameplay Events
        // Subscribe to Gameplay Events
        SubscribeToEvents();

        // Initialize Volume
        float savedVol = PlayerPrefs.GetFloat("MasterVolume", 1f);
        AudioListener.volume = savedVol;
    }

    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= OnSceneChanged;
            UnsubscribeFromEvents();
        }
    }

    private void SubscribeToEvents()
    {
        PlayerShooting.OnShoot += PlayShootSound;
        Arrow.OnEnemyHit += PlayEnemyHitSound;
        PlayerHealth.OnPlayerDamage += PlayPlayerHitSound;
        PlayerHealth.OnPlayerDeath += PlayPlayerDeathSound;
    }
    
    // Safety unsubscribe
    private void UnsubscribeFromEvents()
    {
        PlayerShooting.OnShoot -= PlayShootSound;
        Arrow.OnEnemyHit -= PlayEnemyHitSound;
        PlayerHealth.OnPlayerDamage -= PlayPlayerHitSound;
        PlayerHealth.OnPlayerDeath -= PlayPlayerDeathSound;
    }

    private void OnSceneChanged(Scene current, Scene next)
    {
        PlayMusicForScene(next.name);
    }
    
    // ... rest of file logic is fine, ensured PlayPlayerDeathSound is private action-compatible or just method.
    // I defined it as public previously, but I can match the others.
    
    private void PlayMusicForScene(string sceneName)
    {
        if (sceneName == "DungeonScene")
        {
            if (dungeonMusic != null && (musicSource.clip != dungeonMusic || !musicSource.isPlaying))
            {
                musicSource.clip = dungeonMusic;
                musicSource.loop = true;
                musicSource.Play();
            }
        }
        else
        {
            // Stop music in MainMenu or others
            musicSource.Stop();
        }
    }

    private void PlaySound(SoundConfig config)
    {
        if (config == null || config.clip == null || sfxSource == null) return;

        sfxSource.pitch = Random.Range(config.minPitch, config.maxPitch);
        sfxSource.PlayOneShot(config.clip);
    }

    public void PlayClick() => PlaySound(clickSound);
    public void PlayHover() => PlaySound(hoverSound);
    
    private void PlayShootSound() => PlaySound(shootSound);
    private void PlayPlayerHitSound() => PlaySound(playerHitSound);
    private void PlayEnemyHitSound() => PlaySound(enemyHitSound);
    private void PlayPlayerDeathSound() => PlaySound(playerDeathSound);
}
