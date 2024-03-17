using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineUpSprites : MonoBehaviour
{

    public float spacing = 2f; 

    void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            // Position each sprite next to each other with specified spacing
            transform.GetChild(i).transform.position = new Vector3(i * spacing, 0, 0);
        }
    }
}
