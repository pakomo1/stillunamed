using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public static class OnClickFile
{
   
    
    public static void OnPointerClick(GameObject obj, ReadFiles readFiles)
    {
       
        string fileName = obj.name;
       /* var tmp = obj.GetComponent<TextMeshProUGUI>();
        tmp.color = Color.red;*/
        
        readFiles.selectedFilePath = readFiles.currentWorkingDir+"/"+fileName;
    }

}
