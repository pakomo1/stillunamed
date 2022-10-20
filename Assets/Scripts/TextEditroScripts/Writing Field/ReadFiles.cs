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
