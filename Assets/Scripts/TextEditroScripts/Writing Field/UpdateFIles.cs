using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Diagnostics;
using System;

public class UpdateFIles : MonoBehaviour
{
    private ReadFiles readFiles;
    [SerializeField] private GameObject TextFieldManager;

    private MonoCompiler compiler;
    [SerializeField] private GameObject canvas;
   
    [SerializeField] public Button compileButton;
    private string EXEfile;
    private string path;

    //these two come form the ReadFiles class
    private string selectedFilePath;
    private TMP_InputField inputField;

    public string currentSaveDate;
    // Start is called before the first frame update
    void Start()
    {
        readFiles = TextFieldManager.GetComponent<ReadFiles>();
        compiler = canvas.GetComponent<MonoCompiler>();

        this.selectedFilePath = readFiles.selectedFilePath;
        this.inputField = readFiles.inputfield;
        this.EXEfile = readFiles.EXEfile;   
        this.path = readFiles.path; 
    }

    // Update is called once per frame
     void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
          if(CheckIfDifferent(inputField.text, File.ReadAllText(selectedFilePath)))
           {
                 File.WriteAllText(selectedFilePath, inputField.text);
                // THIS IS STARTING MONO COMPILER. It also executes a csc command that creates the exe file
                CreateExeFile(selectedFilePath);

                if (File.Exists(EXEfile))
                {
                    compiler.MyMove(EXEfile, path);
                }
               

                //compiler.MyMove(EXEfile, path);
                compiler.EnableAndDisableButton(compileButton, 1000);
                print("The file has been saved");
                currentSaveDate = DateTime.Now.ToString();
            }
        }
    }

    public bool CheckIfDifferent(string text1, string text2)
    {
        if(text1 != text2)
        {
            return true;
        }

        return false;
    }
    public void CreateExeFile(string pathToFile)
    {
        string outputFilePath = "C:\\Users\\Maixm\\Documents\\output.txt";
        Process mono = new Process();
        mono.StartInfo.FileName = "cmd.exe";
        mono.StartInfo.WorkingDirectory = @"C:\Program Files\Mono\bin\";
        mono.StartInfo.Arguments = $@"/C csc " + pathToFile + " > " + outputFilePath;
        mono.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        mono.StartInfo.UseShellExecute = true; 
        mono.StartInfo.RedirectStandardOutput = true;
        mono.StartInfo.RedirectStandardError = true;
        mono.StartInfo.UseShellExecute = false;

        Thread processThread = new Thread(() =>
        {
            mono.OutputDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    File.WriteAllText(outputFilePath, args.Data);
                }
            };

            mono.ErrorDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    File.WriteAllText(outputFilePath, args.Data);
                }
            };

            mono.Start();
            mono.BeginOutputReadLine();
            mono.BeginErrorReadLine();
            if (!mono.WaitForExit(10000))
            {
                // If the process hasn't exited within 10 seconds, kill it
                mono.Kill();
            }
        });
        processThread.IsBackground = true;
        processThread.Start();
    }
}
