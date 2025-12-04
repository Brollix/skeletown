using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtonFeedback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Settings")]
    [SerializeField] private float scaleAmount = 1.1f;
    [SerializeField] private float duration = 0.1f;
    [SerializeField] private Color hoverColor = Color.yellow;
    
    [Header("Audio")]
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;
    
    private Vector3 originalScale;
    private Color originalColor;
    private Image targetImage;

    private void Awake()
    {
        originalScale = transform.localScale;
        targetImage = GetComponent<Image>();
        if (targetImage != null)
        {
            originalColor = targetImage.color;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleTo(originalScale * scaleAmount));
        if (targetImage != null) targetImage.color = hoverColor;
        
        // Play hover sound
        if (hoverSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(hoverSound, 1f, 1f); // No pitch variation for UI
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleTo(originalScale));
        if (targetImage != null) targetImage.color = originalColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleTo(originalScale * 0.9f));
        
        // Play click sound
        if (clickSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(clickSound, 1f, 1f);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleTo(originalScale * scaleAmount)); // Return to hover scale
    }

    private System.Collections.IEnumerator ScaleTo(Vector3 target)
    {
        float elapsed = 0f;
        Vector3 start = transform.localScale;
        
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime; // Use unscaled time for UI
            transform.localScale = Vector3.Lerp(start, target, elapsed / duration);
            yield return null;
        }
        transform.localScale = target;
    }
}
