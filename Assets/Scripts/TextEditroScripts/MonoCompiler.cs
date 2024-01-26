using UnityEngine;
using System;
using System.IO;
using System.Diagnostics;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Collections;

public class MonoCompiler : MonoBehaviour
{
    [SerializeField] private TextEditor textEditor;
    private UpdateFIles updateFiles;
    [SerializeField] private GameObject TextFieldManager;


    [SerializeField] public Button runButton;
    private string selectedFilePath;

    private string nameOfFile;
    private string EXEfile;
    private string currentWorkingDir;
    private string pathToFile;

    private string currentSaveDate;

    [SerializeField] private Color wantedColor;
    // Start is called before the first frame update
    void Start()
    {
        updateFiles = TextFieldManager.GetComponent<UpdateFIles>();
    }

    // Update is called once per frame
    void Update()
    {
        selectedFilePath = textEditor.PathToTheSelectedFile;
        EXEfile = textEditor.PathToSelectedExeFile;
        currentWorkingDir = textEditor.WorkingDirectory;

        currentSaveDate = updateFiles.currentSaveDate;
    }
    public void Compile()
    {
        string pathOfExeFile = $"{currentWorkingDir}\\{nameOfFile}.exe";
        string DateThatExeFileCreated = File.GetCreationTime(pathOfExeFile).ToString();
       
        if (File.Exists(EXEfile))
        {
            MyMove(EXEfile, currentWorkingDir);
        }

        //This method is moving the .exe file because for some reason its been generated in the mono folder instead of the folder that the cs file is
        //This method is executing the exe file
        LaunchCommandLineApp(nameOfFile, currentWorkingDir);
        EnableAndDisableButton(runButton, 2000);
    }

    public void MyMove(string filePath, string targetDir)
    {
        try
        {
            if (!File.Exists($@"{targetDir}\{Path.GetFileName(filePath)}"))
            {
            File.Move(filePath, $@"{targetDir}\{Path.GetFileName(filePath)}");
                print("Your file has been moved");
            }
            else
            {
                File.Replace(filePath, @$"{targetDir}\{Path.GetFileName(filePath)}", null);
                print("Your file has been replaced");
            }
        }
        catch (Exception ex)
        {
            print(ex.Message);
        }
    }
    static public void LaunchCommandLineApp(string fileName, string pathToFile)
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
            //The code below kills the process created so it does not take memory(as a parameter we should pass the name of the exe file)
            Process[] processes = Process.GetProcessesByName("file");
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
   async public void EnableAndDisableButton(Button btn, int seconds)
    {
        btn.enabled = false;
        ColorBlock cb = btn.colors;
        var oldNMcolor = cb.normalColor;
        var HighLcolor = cb.highlightedColor;
        var PressedColor = cb.pressedColor;

        cb.normalColor = wantedColor;
        cb.highlightedColor = wantedColor;
        cb.pressedColor = wantedColor;
        btn.colors = cb;

        await Task.Delay(seconds);
        btn.enabled = true;

        cb.normalColor = oldNMcolor;
        cb.highlightedColor = HighLcolor;
        cb.pressedColor = PressedColor;
        btn.colors = cb;
    }
    
}