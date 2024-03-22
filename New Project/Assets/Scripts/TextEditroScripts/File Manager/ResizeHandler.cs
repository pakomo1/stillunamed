using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResizeHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private float offset;
    private bool isResizing = false;
    private RectTransform fileManagerRectTransform;

    [SerializeField]
    private RectTransform textAreaRectTransform; // Serialized field for the RectTransform of the text editor area

    [SerializeField]
    private float minWidth = 100f; // Minimum width of the FileManager

    [SerializeField]
    private float maxWidth = 500f; // Maximum width of the FileManager

    private void Start()
    {
        // Get the RectTransform from the parent (FileManager)
        fileManagerRectTransform = transform.parent.GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isResizing = true;
        offset = fileManagerRectTransform.rect.width - eventData.position.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isResizing = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isResizing)
        {
            float mouseX = eventData.position.x;
            float newWidth = mouseX + offset;

            // Clamp the new width to the minimum and maximum values
            newWidth = Mathf.Clamp(newWidth, minWidth, maxWidth);

            fileManagerRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);

            // Adjust the left padding of the Text Area based on the width of the FileManager
            textAreaRectTransform.offsetMin = new Vector2(newWidth, textAreaRectTransform.offsetMin.y);
        }
    }
}
