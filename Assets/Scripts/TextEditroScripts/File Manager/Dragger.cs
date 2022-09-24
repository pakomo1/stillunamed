using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragger : MonoBehaviour
{
   //  private Vector2 mousePosition = new Vector2();
    public Transform Field;
    public Transform filemenu;
    public float maxWidthLeft;
    public float maxHeightLeft;


    //IDragHandler

    // Start is called before the first frame update
    void Start()
    {
       
        transform.localScale += new Vector3(1, 0, 0);
        transform.position += new Vector3(1, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (filemenu.position.x > maxWidthLeft || filemenu.position.x < maxWidthLeft)
        {
            Debug.Log(filemenu.position);
            filemenu.position = new Vector3(maxWidthLeft, filemenu.position.y, filemenu.position.z);
        }

    }
   
     

}
