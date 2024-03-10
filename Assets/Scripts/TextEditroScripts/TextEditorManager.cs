using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using TMPro;
using System.IO;
using System;
using System.Threading.Tasks;
using UnityEngine.EventSystems;

public class TextEditorManager : MonoBehaviour
{
    private InGameTextEditor.TextEditor textEditor;
    private string username;
    private string startingDirecotry;
    private string displayText;
    private string pathToTheSelectedFile;
    private string wokringDirectory;

    public delegate void DisplayTextChangedHandler(string newText);
    public event DisplayTextChangedHandler OnDisplayTextChanged;

    public delegate void SelectedFileChangedHandler(string newText);
    public event SelectedFileChangedHandler OnSelectedFileChanged;

    // the directroy we in which we are currently in
    public string WorkingDirectory
    {
        get { return wokringDirectory; }
        set { wokringDirectory = value; }
    }
    //the path to the exe file of the selected file
    public string StartingDirecotry
    {
        get { return startingDirecotry; }
        set { startingDirecotry = value; }
    }
    //the user that this editor belongs to
    public string UserName
    {
        get { return username; }
    }
    //the text that is supposed to be displayed in the inputField
    public string DisplayText
    {
        get { return displayText; }
        set { displayText = value; }
            
    }
    //the path to the selected file
    public string PathToTheSelectedFile
    {
        get { return pathToTheSelectedFile; }
        set
        {
            if (pathToTheSelectedFile != value)
            {
                pathToTheSelectedFile = value;
                // Trigger the event
                OnSelectedFileChanged?.Invoke(pathToTheSelectedFile);
            }
        }
    }
    public TextEditorManager(string userName, string startingDirecotry,string displayText)
    {
        username = userName;
        StartingDirecotry = startingDirecotry;
        DisplayText = displayText;
    }
    private void Start()
    {
        OnSelectedFileChanged += OnFileSelectedHandlerAsync;
        textEditor = transform.GetChild(0).GetComponent<InGameTextEditor.TextEditor>();
    }
    private void Update()
    {
    }

    private async void OnFileSelectedHandlerAsync(string filePath)
    {
         string fileContent = await File.ReadAllTextAsync(filePath);
         print(fileContent);
         textEditor.SetText(fileContent);
    }
}
