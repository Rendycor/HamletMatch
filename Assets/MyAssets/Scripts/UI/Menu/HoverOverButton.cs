using UnityEngine;
using UnityEngine.EventSystems;

public class HoverOverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject hoverImage;

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverImage.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        hoverImage.SetActive(false);
    }
}
