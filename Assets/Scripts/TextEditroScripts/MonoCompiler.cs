using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Diagnostics;
using System.Threading;

public class MonoCompiler : MonoBehaviour
{

    private ReadFiles readFiles;
    [SerializeField] private GameObject FileReader;

    private string selectedFilePath;
    // Start is called before the first frame update
    void Start()
    {
        readFiles= FileReader.GetComponent<ReadFiles>();
    }

    // Update is called once per frame
    void Update()
    {
        selectedFilePath = readFiles.selectedFilePath;
    }
    public void Compile()
    {
        //this gets the name of the file plus the ... thingie (.exe; .cs; .js)
        string name = selectedFilePath.Substring(selectedFilePath.Length - 7);
        //this will get only the name of the file
        string nameOfFile = name.Substring(0, name.IndexOf('.'));


        //this code here gets the directory that the selected file(selectedFilePath)
        string pathToFile = selectedFilePath.Substring(0, selectedFilePath.LastIndexOf('\\'));

        string EXEfile = @$"C:\Program Files\Mono\bin\{nameOfFile}.exe";
        string path = @$"{pathToFile}";

        // THIS IS STARNG MONO COMPILER
        Process mono = new Process();
        mono.StartInfo.FileName = "cmd.exe";
        mono.StartInfo.WorkingDirectory = @"C:\Program Files\Mono\bin\";
        mono.StartInfo.Arguments = $@"/K csc " + selectedFilePath;
        mono.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        mono.Start();

        //This method is moving the exefile becouse for some reason its been generated in the mono folder instead of the folder that the cs file is
        MyMove(EXEfile, path);
        //This method is executing the exe file
        LaunchCommandLineApp(nameOfFile, pathToFile);

    }
    static public bool MyMove(string filePath, string targetDir)
    {
        try
        {
            if (!File.Exists($@"{targetDir}\{Path.GetFileName(filePath)}"))
            {
            File.Move(filePath, $@"{targetDir}\{Path.GetFileName(filePath)}");

            }
            else{
                File.Replace(filePath, @$"{targetDir}\{Path.GetFileName(filePath)}", null);
            }
            return true;
        }
        catch (Exception ex)
        {
            print(ex.Message);
            return false;
        }
    }
    static void LaunchCommandLineApp(string fileName, string pathToFile)
    {
        try
        {
            var process = new Process
            {

                StartInfo = new ProcessStartInfo
                {
                    FileName = @$"{pathToFile}\{fileName}.exe",
                    Arguments = "behavior query SymlinkEvaluation",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            process.Start();
            int id = process.Id;

         
            while (!process.StandardOutput.EndOfStream)
            {
                var line = process.StandardOutput.ReadLine();
                print("Your output is: " + line);
            }
            //The code below kills the process created so it does not take memory 
            Process[] processes = Process.GetProcessesByName("hello");
            foreach (Process proc in processes)
            {
                proc.Kill();
            }
        }
        catch (Exception err)
        {
            print(err.Message);
        }

    }
}
