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
        
        PlayMusicForScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        SubscribeToEvents();

        float musicVol = PlayerPrefs.GetFloat(GameConstants.PREF_MUSIC_VOLUME, 1f);
        float sfxVol = PlayerPrefs.GetFloat(GameConstants.PREF_SFX_VOLUME, 1f);
        bool musicMute = PlayerPrefs.GetInt(GameConstants.PREF_MUSIC_MUTED, 0) == 1;
        bool sfxMute = PlayerPrefs.GetInt(GameConstants.PREF_SFX_MUTED, 0) == 1;

        if (musicSource != null)
        {
            musicSource.volume = musicVol;
            musicSource.mute = musicMute;
        }

        if (sfxSource != null)
        {
            sfxSource.volume = sfxVol;
            sfxSource.mute = sfxMute;
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = volume;
            PlayerPrefs.SetFloat(GameConstants.PREF_MUSIC_VOLUME, volume);
        }
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = volume;
            PlayerPrefs.SetFloat(GameConstants.PREF_SFX_VOLUME, volume);
        }
    }

    public void ToggleMusic(bool isOn)
    {
        if (musicSource != null)
        {
            musicSource.mute = !isOn; 
            PlayerPrefs.SetInt(GameConstants.PREF_MUSIC_MUTED, !isOn ? 1 : 0);
        }
    }

    public void ToggleSFX(bool isOn)
    {
        if (sfxSource != null)
        {
            sfxSource.mute = !isOn;
            PlayerPrefs.SetInt(GameConstants.PREF_SFX_MUTED, !isOn ? 1 : 0);
        }
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
    
    private void PlayMusicForScene(string sceneName)
    {
        if (sceneName == GameConstants.SCENE_DUNGEON)
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
