using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[RequireComponent(typeof(CanvasGroup), typeof(Image))]
public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas parentCanvas;
    private CanvasGroup canvasGroup;
    private Image itemImage;
    private Vector3 originalPosition;
    private Transform originalParent;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        itemImage = GetComponent<Image>();
        parentCanvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return; // Left-click only
        StoreOriginalPosition();
        SetDraggingState(true);
        MoveToTopLayer();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return; // Left-click only
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return; // Left-click only
        InventorySlot slot = GetSlot();
        if (slot == null)
            ResetPosition();
        else
            SwapItems(slot);

        SetDraggingState(false);
    }

    private void StoreOriginalPosition()
    {
        originalPosition = transform.position;
        originalParent = transform.parent;
    }

    private void SetDraggingState(bool isDragging)
    {
        canvasGroup.alpha = isDragging ? 0.6f : 1f;
        canvasGroup.blocksRaycasts = !isDragging;
        itemImage.raycastTarget = !isDragging;
    }

    private void MoveToTopLayer()
    {
        transform.SetParent(parentCanvas.transform);
        transform.SetAsLastSibling();
    }

    private void ResetPosition()
    {
        transform.SetParent(originalParent);
        transform.position = originalPosition;
    }

    private void SwapItems(InventorySlot slot)
    {
        if (slot.transform.childCount > 0)
        {
            Transform swapItem = slot.transform.GetChild(0);
            swapItem.SetParent(originalParent);
            swapItem.localPosition = Vector3.zero;
        }

        transform.SetParent(slot.transform);
        transform.localPosition = Vector3.zero;
    }

    private InventorySlot GetSlot()
    {
        PointerEventData eventData = new(EventSystem.current) { position = Input.mousePosition };
        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.TryGetComponent(out InventorySlot slot))
                return slot;
        }
        return null;
    }
}