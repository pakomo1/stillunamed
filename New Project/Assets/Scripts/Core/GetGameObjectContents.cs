using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public static class GetGameObjectContents
{
    public static List<GameObject> GetContent(GameObject contentHolder)
    {
        List<GameObject> elements = new List<GameObject>();
        int totalElements;
        totalElements = contentHolder.transform.childCount;
        for (int i = 0; i < totalElements; i++)
        {
            elements.Add(contentHolder.transform.GetChild(i).gameObject);
        }
        return elements;
    }
}
