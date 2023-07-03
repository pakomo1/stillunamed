using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public static class OnClickFile 
{
    

    public static void OnPointerClick(GameObject obj)
    {
        Debug.Log(obj.name);
    }

}
