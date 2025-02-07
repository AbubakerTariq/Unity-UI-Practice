using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image slotImage;
    private Color defaultColor;
    [SerializeField] private Color highlightColor;

    private void Start()
    {
        slotImage = GetComponent<Image>();
        defaultColor = slotImage.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        slotImage.color = highlightColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        slotImage.color = defaultColor;
    }
}