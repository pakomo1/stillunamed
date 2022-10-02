using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using System.IO;

public class ReadFiles : MonoBehaviour
{

    [SerializeField] private TMP_InputField inputfield;
    // Start is called before the first frame update
    void Start()
    {
        //should take the path that has been selected form the Open Folder or the dir
        //for now i am just going to use a static file form a static dir

          string[] files = Directory.GetFiles(@"D:\");



        List<string> lines = File.ReadAllLines(@"C:\Users\Maixm\Documents\file.txt").ToList();
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
