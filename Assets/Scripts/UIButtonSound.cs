using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{

    // Plays sound when pointer enters the element.
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayHover();
        }
    }


    // Plays sound when pointer clicks the element.
    public void OnPointerClick(PointerEventData eventData)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayClick();
        }
    }
}
