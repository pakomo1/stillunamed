using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public class OpenFolder : MonoBehaviour
{

   public string path;
    public string[] files;
    [SerializeField]private RawImage image;

    public void OpenExplorer()
    {
        path = EditorUtility.OpenFolderPanel("Overwrite with folders","","All folders");
        files = Directory.GetFiles(path);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
