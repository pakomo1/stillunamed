using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dragger : MonoBehaviour, IDragHandler,IEndDragHandler
{
   //  private Vector2 mousePosition = new Vector2();
    [SerializeField] private RectTransform Field;
    [SerializeField] private RectTransform filemenu;
    [SerializeField] private float maxWidthLeft;
    [SerializeField] private float maxHeightLeft;

    public void OnDrag(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {

        }
        filemenu.offsetMax += new Vector2 (10, 0);
        print(filemenu.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        filemenu.transform.localPosition = Vector3.zero;
    }



    //IDragHandler

    // Start is called before the first frame update
    void Start()
    {

        //transform.localScale += new Vector3(1, 0, 0);
        //transform.position += new Vector3(1, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        filemenu.right += new Vector3(1, 0, 0);
       
       // Debug.Log (filemenu.offsetMax);
        //offsetMax this is the upper right corner
        if(filemenu.offsetMax.x < 2.86)
        {
            filemenu.offsetMax = new Vector2((float)8.86, filemenu.offsetMax.y);
        }
        if(filemenu.offsetMax.y != 8.28)
        {
            filemenu.offsetMax = new Vector2(filemenu.offsetMax.x, (float)8.28);
        }
        // Cursor.SetCursor(cursor, filemenu.offsetMax, CursorMode.Auto);
        
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            //this.filemenu.transform.right = new Vector3(1, 0, 0);

    }
}
