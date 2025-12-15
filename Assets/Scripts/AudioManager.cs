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
    //This makes sure that there is only one audio manager present across all scenes so there are no duplicates and it makes sure it isn't destroyed when changing scenes.

    // Sets up the Singleton instance and persistent behavior.
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
    //This method ensures that the audio clips play when certain events in the game happen. When an event happens, the manager catches it and plays the corresponding sound.

    // Subscribes to events and initializes music/volume.
    private void Start()
    {
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChanged;
        
        PlayMusicForScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        SubscribeToEvents();

        float savedVol = PlayerPrefs.GetFloat("MasterVolume", 1f);
        AudioListener.volume = savedVol;
    }
    //This method sets the master volume for all game audio. SFX and Music.

    // Sets the global audio volume.
    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
    }
    //When the audio manager is destroyed, it unsubscribes from events.

    // Unsubscribes from events when destroyed to prevent memory leaks.
    private void OnDestroy()
    {
        if (Instance == this)
        {
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= OnSceneChanged;
            UnsubscribeFromEvents();
        }
    }


    // Listens for specific gameplay events to trigger sound effects.
    private void SubscribeToEvents()
    {
        PlayerShooting.OnShoot += PlayShootSound;
        Arrow.OnEnemyHit += PlayEnemyHitSound;
        PlayerHealth.OnPlayerDamage += PlayPlayerHitSound;
        PlayerHealth.OnPlayerDeath += PlayPlayerDeathSound;
    }
    
    //This method unsubscribes the manager from the game events. It's called above when the manager is destroyed.

    // Stops listening for gameplay events.
    private void UnsubscribeFromEvents()
    {
        PlayerShooting.OnShoot -= PlayShootSound;
        Arrow.OnEnemyHit -= PlayEnemyHitSound;
        PlayerHealth.OnPlayerDamage -= PlayPlayerHitSound;
        PlayerHealth.OnPlayerDeath -= PlayPlayerDeathSound;
    }
    
    //This method updates the background music when the active scene changes.

    // Triggers music updates when the active scene changes.
    private void OnSceneChanged(Scene current, Scene next)
    {
        PlayMusicForScene(next.name);
    }
    
    //This method is called in OnSceneChanged and it makes sure to play the background music when the DungeonScene is active. And it makes sure to stop playing the music when the Main Menu scene is active.

    // Plays or stops music based on the current scene.
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
            musicSource.Stop();
        }
    }

    //This method makes the sound effects play with the pitch configuartions, making the pitch shift between the minimum and maximum values at random every time.

    // Plays a specific sound effect with randomized pitch.
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
