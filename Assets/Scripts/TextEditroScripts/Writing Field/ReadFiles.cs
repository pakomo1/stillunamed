using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.Rendering;
using System.Net;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using System;

public class ReadFiles : MonoBehaviour
{

    [SerializeField] private TMP_InputField inputfield;
    [SerializeField] private TextEditor textEditor;
    private OpenFolder openfolder;
    private string selectedFilePath;
    public string nameOfFilePlus;
    public string nameOfFile;
    public string EXEfile;
    public string currentWorkingDir;

    public string previousSelectedFile;
    
    void Start()
    {
        //i should take the file form the file manager since there you select which file you want to read
        //for now i am just going to use a static file form a static dir

        //also a VERY IMPORTANT THING that you should not forget!!!
        /*
         * 
         * you can select files only ONLY form the directory you are in and that is the path that you select in the OpenFolder.cs file
         * 
         */
        openfolder = FindObjectOfType<OpenFolder>();
        
        
        textEditor.PathToTheSelectedFile = "C:\\Users\\Maixm\\Documents\\file.cs";
        selectedFilePath = textEditor.PathToTheSelectedFile;

        // gets the name of the file plus the ... thingie (.exe; .cs; .js)
        string name = selectedFilePath.Substring(selectedFilePath.Length - 7);
        this.nameOfFilePlus = name;
        // get only the name of the file
        string nameOfFile = name.Substring(0, name.IndexOf('.'));
        this.nameOfFile = nameOfFile;
        // gets the directory that the selected file(selectedFilePath)
        string pathToFile = selectedFilePath.Substring(0, selectedFilePath.LastIndexOf('\\'));
        
        string EXEfile = @$"C:\Program Files\Mono\bin\{nameOfFile}.exe";
        textEditor.PathToSelectedExeFile = EXEfile;


      //  string[] files = Directory.GetFiles(@"D:\");

        textEditor.DisplayText = File.ReadAllText(selectedFilePath);

    }

    // Update is called once per frame
    void Update()
    {
        if (selectedFilePath != "")
        {
            var selectedFileObj = GameObject.Find(Path.GetFileName(selectedFilePath));

            TextMeshProUGUI selectedfileTmpPro;
            selectedFileObj.TryGetComponent<TextMeshProUGUI>(out selectedfileTmpPro);
            selectedfileTmpPro.color = Color.red;

        }

        if (selectedFilePath != previousSelectedFile && previousSelectedFile != "")
        {
            try
            {
                print(previousSelectedFile);
                var previousSelectedFileOBJ = GameObject.Find(Path.GetFileName(previousSelectedFile));

                TextMeshProUGUI fileTmpPro;
                previousSelectedFileOBJ.TryGetComponent<TextMeshProUGUI>(out fileTmpPro);
                fileTmpPro.color = Color.white;

                previousSelectedFile = selectedFilePath;
                textEditor.DisplayText= File.ReadAllText(selectedFilePath);
            }
            catch (Exception ex)
            {
                print(ex);
            }
           
        }
    }
}
