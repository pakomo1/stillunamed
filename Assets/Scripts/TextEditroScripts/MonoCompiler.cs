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
        string name = selectedFilePath.Substring(selectedFilePath.Length - 7);
        string nameOfFile = name.Substring(0, name.IndexOf('.'));


        string EXEfile = @$"C:\Program Files\Mono\bin\{nameOfFile}.exe";
        string path = @"C:\";

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
        LaunchCommandLineApp();

    }
    static public bool MyMove(string filePath, string targetDir)
    {
        try
        {
            File.Move(filePath, $"{targetDir}{Path.GetFileName(filePath)}");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
    static void LaunchCommandLineApp()
    {
        try
        {
            var process = new Process
            {

                StartInfo = new ProcessStartInfo
                {
                    FileName = @"C:\file.exe",
                    Arguments = "behavior query SymlinkEvaluation",
                    UseShellExecute = false,

                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            process.Start();
            int id = process.Id;

            //The code below kills the process created so it does not take memory 
            Process[] processes = Process.GetProcessesByName("hello");
            foreach (Process proc in processes)
            {
                proc.Kill();
            }
            while (!process.StandardOutput.EndOfStream)
            {
                var line = process.StandardOutput.ReadLine();
                print("Your output is: " + line);
            }
        }
        catch (Exception err)
        {
            Console.WriteLine(err.Message);
        }

    }
}
