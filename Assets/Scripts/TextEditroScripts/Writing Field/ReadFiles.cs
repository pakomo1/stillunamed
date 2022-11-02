using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using System.IO;

public class ReadFiles : MonoBehaviour
{

    [SerializeField] public TMP_InputField inputfield;
    public string selectedFilePath;
    public string nameOfFilePlus;
    public string nameOfFile;
    public string pathToFile;
    public string EXEfile;
    public string path;
    // Start is called before the first frame update
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
        selectedFilePath =@"C:\Users\Maixm\Documents\file.cs";

        //this gets the name of the file plus the ... thingie (.exe; .cs; .js)
        string name = selectedFilePath.Substring(selectedFilePath.Length - 7);
        this.nameOfFilePlus = name;
        //this will get only the name of the file
        string nameOfFile = name.Substring(0, name.IndexOf('.'));
        this.nameOfFile = nameOfFile;
        //this code here gets the directory that the selected file(selectedFilePath)
        string pathToFile = selectedFilePath.Substring(0, selectedFilePath.LastIndexOf('\\'));
        this.pathToFile = pathToFile;

        string EXEfile = @$"C:\Program Files\Mono\bin\{nameOfFile}.exe";
        this.EXEfile = EXEfile;

        string path = @$"{pathToFile}";
        this.path = path;


        string[] files = Directory.GetFiles(@"D:\");

        inputfield.text = File.ReadAllText(selectedFilePath);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
