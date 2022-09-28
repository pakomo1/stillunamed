using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class OpenFolder : MonoBehaviour
{
    private TheManager theManager;
    public string path;
   // [SerializeField]private RawImage image;


    private void Awake()
    {
        theManager = FindObjectOfType<TheManager>();
    }
    public void OpenExplorer()
    {
        path = EditorUtility.OpenFolderPanel("Overwrite with folders","","All folders");
        theManager.UpdatePath(path);

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
