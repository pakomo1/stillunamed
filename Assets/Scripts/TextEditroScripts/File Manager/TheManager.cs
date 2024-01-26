using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class TheManager : MonoBehaviour
{
    private string currentWorkingDir;
    public string[] files;
    public string[] folders;
    [SerializeField] private GameObject fileManager;
    [SerializeField] private GameObject TextFieldManager;
    [SerializeField] private TextEditor textEditor;
    private OpenFolder openfolder;
    // Start is called before the first frame update
    void Start()
    {
        openfolder = TextFieldManager.GetComponent<OpenFolder>();
    }

    // Update is called once per frame
    void Update()
    {

        if(textEditor.StartingDirecotry != null && textEditor.StartingDirecotry!= "")
        {
            GameObject parent = fileManager;
            int countOfParentObjs = parent.transform.childCount;
            
            folders = Directory.GetDirectories(textEditor.StartingDirecotry);
            foreach (string folder in folders)
            {
            }
            
            files = Directory.GetFiles(textEditor.StartingDirecotry);
            
            foreach (string file in files)
            {
               
            }

        }
       
    }


}
