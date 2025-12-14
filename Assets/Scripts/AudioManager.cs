using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music Clips")]
    [SerializeField] private AudioClip dungeonMusic;

    [Header("UI Sounds")]
    [SerializeField] private AudioClip globalClickSound;

    public void PlayClickSound()
    {
        if (globalClickSound != null && sfxSource != null)
        {
            sfxSource.pitch = 1.0f; 
            sfxSource.PlayOneShot(globalClickSound);
        }
    }

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Subscribe to scene changes to handle music
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChanged;
        
        // Initial check
        PlayMusicForScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    
    private void OnDestroy() 
    {
        // Vital check: duplicate instances being destroyed should NOT unsubscribe the main instance's events!
        if (Instance == this)
        {
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= OnSceneChanged;
        }
    }

    private void OnSceneChanged(Scene current, Scene next)
    {
        PlayMusicForScene(next.name);
    }

    private void PlayMusicForScene(string sceneName)
    {
        if (sceneName == "DungeonScene")
        {
            if (dungeonMusic != null && musicSource.clip != dungeonMusic)
            {
                musicSource.clip = dungeonMusic;
                musicSource.loop = true;
                musicSource.Play();
            }
        }
        else if (sceneName == "MainMenu")
        {
            // Stop music in menu (or play something else)
            musicSource.Stop();
        }
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip != null && sfxSource != null)
        {
            // Randomize pitch slightly for variety
            sfxSource.pitch = Random.Range(0.9f, 1.1f);
            sfxSource.PlayOneShot(clip, volume);
        }
    }
}
