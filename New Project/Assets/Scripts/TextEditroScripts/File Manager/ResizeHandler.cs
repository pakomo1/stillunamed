using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResizeHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private float offset;
    private bool isResizing = false;
    private LayoutElement layoutElement;

    private void Start()
    {
        layoutElement = transform.parent.GetComponent<LayoutElement>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isResizing = true;
        offset = layoutElement.preferredWidth - eventData.position.x;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isResizing = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isResizing)
        {
            float mouseX = eventData.position.x;
            float newWidth = mouseX + offset;
            layoutElement.preferredWidth = newWidth;
        }
    }
}
