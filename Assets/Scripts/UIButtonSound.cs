using UnityEngine;

public class UIButtonSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;

    public void PlayClick()
    {
        // Try global first, fall back to local
        if (AudioManager.Instance != null && AudioManager.Instance.GetInstanceID() != 0) // Extra safety check
        {
            AudioManager.Instance.PlayClickSound();
        }
        else if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
