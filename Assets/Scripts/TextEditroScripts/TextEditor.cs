using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using TMPro;
using System.IO;
using System;
using System.Threading.Tasks;

public class TextEditor : MonoBehaviour
{
    private string userName;
    private string startingDirecotry;
    private string displayText;
    private string pathToTheSelectedFile;
    private string pathToSelectedExeFile;
    private string wokringDirectory;

    [SerializeField]private TMP_InputField inputField;

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
        get { return userName; }
        set { userName = value; }
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
        set { pathToTheSelectedFile = value; }
    }
    public TextEditor(string userName, string startingDirecotry,string displayText)
    {
        UserName = userName;
        StartingDirecotry = startingDirecotry;
        DisplayText = displayText;
    }
    private void Start()
    {
        TheManager.OnFileSelected += OnFileSelectedHandler; 
    }
    private void Update()
    {
        inputField.text = DisplayText;
    }

    private async void OnFileSelectedHandler(object sender, EventArgs args)
    {
        string selectdFileContent =await File.ReadAllTextAsync(PathToTheSelectedFile);
        DisplayText = selectdFileContent;
    }
}
