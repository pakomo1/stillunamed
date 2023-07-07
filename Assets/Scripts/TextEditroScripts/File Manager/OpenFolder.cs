using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using System.IO;

public class OpenFolder : MonoBehaviour
{
    private TheManager theManager;
    private ReadFiles readFiles;
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [SerializeField] private GameObject fileManager;    
    public string _currentWorkingDir;
   // [SerializeField]private RawImage image;


    private void Awake()
    {
        theManager = FindObjectOfType<TheManager>();
        readFiles= FindObjectOfType<ReadFiles>();
    }
    public void OpenExplorer()
    {
        ClearAllFields();
        _currentWorkingDir = EditorUtility.OpenFolderPanel("Overwrite with folders","","All folders");
        theManager.UpdatePath(_currentWorkingDir);
        _textMeshPro.text = _currentWorkingDir;

        //set the default valuses for:
        readFiles.currentWorkingDir = _currentWorkingDir;
        readFiles.selectedFilePath = Directory.GetFiles(_currentWorkingDir)[0];
        readFiles.previousSelectedFile = readFiles.selectedFilePath;

        print("The first file should be: " +readFiles.previousSelectedFile);
    }
    private void ClearAllFields()
    {
        //clears the file manager field
        var children = new List<GameObject>();
        foreach (Transform child in fileManager.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
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
