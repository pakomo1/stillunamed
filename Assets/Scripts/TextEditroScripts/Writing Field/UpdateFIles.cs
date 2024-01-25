using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Diagnostics;
using System;
using System.Linq.Expressions;
using System.Collections;
using System.Threading.Tasks;

public class UpdateFIles : MonoBehaviour
{
    [SerializeField] private TextEditor textEditor; 
    private ReadFiles readFiles;
    [SerializeField] private GameObject TextFieldManager;

    private MonoCompiler compiler;
    [SerializeField] private GameObject canvas;
   
    [SerializeField] public Button compileButton;
    private string EXEfile;

   [SerializeField] private TMP_InputField inputField;
    private string selectedFilePath;

    public string currentSaveDate;
    // Start is called before the first frame update
    void Start()
    {
        readFiles = TextFieldManager.GetComponent<ReadFiles>();
        compiler = canvas.GetComponent<MonoCompiler>();

        selectedFilePath = textEditor.PathToTheSelectedFile;
        inputField.text = textEditor.DisplayText;
        EXEfile = textEditor.PathToSelectedExeFile;

       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (CheckIfDifferent(inputField.text, File.ReadAllText(selectedFilePath)))
            {
                File.WriteAllText(selectedFilePath, inputField.text);
                // THIS IS STARTING MONO COMPILER. It also executes a csc command that  creates the exe file
                if(Path.GetExtension(selectedFilePath) == ".cs")
                {
                    CreateExeFile(selectedFilePath);
                }

                //compiler.MyMove(EXEfile, path);
                compiler.EnableAndDisableButton(compileButton, 5000);
                print("The file has been saved");
                currentSaveDate = DateTime.Now.ToString();
            }
        }
    }
        public bool CheckIfDifferent(string text1, string text2)
        {
            if (text1 != text2)
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
          //  mono.StartInfo.CreateNoWindow = true;

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
