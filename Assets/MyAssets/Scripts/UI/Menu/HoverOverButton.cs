using UnityEngine;
using UnityEngine.EventSystems;

public class HoverOverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject hoverImage;

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySound2D("Hover");
        hoverImage.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        hoverImage.SetActive(false);
    }
}
