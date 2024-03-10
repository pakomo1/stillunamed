using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public static class OnClickFile
{
   
    
    public static void OnPointerClick(GameObject obj, TextEditorManager textEditorInstance)
    {
        string fileName = obj.name;
       /* var tmp = obj.GetComponent<TextMeshProUGUI>();
        tmp.color = Color.red;*/
        
        textEditorInstance.PathToTheSelectedFile = textEditorInstance.WorkingDirectory+"/"+fileName;
    }

}
