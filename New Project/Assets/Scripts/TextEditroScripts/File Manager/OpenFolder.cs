using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using System.IO;
using UnityEngine.UI;

public class OpenFolder : MonoBehaviour
{
    [SerializeField]private TextEditorData textEditorData;
    [SerializeField]private GenerateForDirectory generateForDirectory;

    [SerializeField] private Button openFolderButton;
    [SerializeField] private GameObject fileManager;
    [SerializeField] private Transform rootPanel;
    private string _currentWorkingDir;
   // [SerializeField]private RawImage image;


    private void Awake()
    {
      openFolderButton.onClick.AddListener(OpenExplorer);
    }
    public void OpenExplorer()
    {
        ClearAllFields();
        textEditorData.StartingDirecotry = EditorUtility.OpenFolderPanel("Overwrite with folders","","All folders");
        
        if(Directory.GetFiles(textEditorData.StartingDirecotry).Length>0)
        {
            textEditorData.PathToTheSelectedFile= Directory.GetFiles(textEditorData.StartingDirecotry)[0];
        }
        textEditorData.WorkingDirectory = textEditorData.StartingDirecotry;
        // textEditor.DisplayText = File.ReadAllText(textEditor.PathToTheSelectedFile);

        generateForDirectory.GenerateForDirectoy(rootPanel, textEditorData.StartingDirecotry);
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
