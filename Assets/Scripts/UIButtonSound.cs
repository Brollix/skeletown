using UnityEngine;

public class UIButtonSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;

    public void PlayClick()
    {
        if (AudioManager.Instance != null && clickSound != null)
        {
            AudioManager.Instance.PlaySFX(clickSound);
        }
        else if (audioSource != null && clickSound != null)
        {
            // Fallback if AudioManager is missing
            audioSource.PlayOneShot(clickSound);
        }
    }
}
