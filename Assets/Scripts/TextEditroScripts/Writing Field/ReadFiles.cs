using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using System.IO;

public class ReadFiles : MonoBehaviour
{

    [SerializeField] private TMP_InputField inputfield;
    public string selectedFilePath;
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

          string[] files = Directory.GetFiles(@"D:\");


        List<string> lines = File.ReadAllLines(selectedFilePath).ToList();
        foreach (string line in lines)
        {
            inputfield.text+= line + "\n";
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
