using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Diagnostics;

public class UpdateFIles : MonoBehaviour
{
    private ReadFiles readFiles;
    private MonoCompiler compiler;

    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject TextFieldManager;
    [SerializeField] public Button runButton;

    private string EXEfile;
    private string path;

    //these two come form the ReadFiles class
    private string selectedFilePath;
    private TMP_InputField inputField;

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
                // THIS IS STARNG MONO COMPILER
                CreateExeFile(selectedFilePath);
               

                //compiler.MyMove(EXEfile, path);
                compiler.EnableAndDisableButton(runButton, 1000);
                print("The file has been saved");
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
        Process mono = new Process();
        mono.StartInfo.FileName = "cmd.exe";
        mono.StartInfo.WorkingDirectory = @"C:\Program Files\Mono\bin\";
        mono.StartInfo.Arguments = $@"/K csc " + pathToFile;
        mono.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        mono.Start();
    }
}
