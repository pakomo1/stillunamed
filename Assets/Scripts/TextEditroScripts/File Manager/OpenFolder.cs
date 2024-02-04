using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using System.IO;
using UnityEngine.UI;

public class OpenFolder : MonoBehaviour
{
    [SerializeField]private TextEditor textEditor;
    [SerializeField]private TheManager theManager;

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
        textEditor.StartingDirecotry = EditorUtility.OpenFolderPanel("Overwrite with folders","","All folders");
        

        textEditor.PathToTheSelectedFile= Directory.GetFiles(textEditor.StartingDirecotry)[0];
        textEditor.WorkingDirectory = textEditor.StartingDirecotry;
       // textEditor.DisplayText = File.ReadAllText(textEditor.PathToTheSelectedFile);

        theManager.GenerateForDirectoy(rootPanel,textEditor.StartingDirecotry);
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
