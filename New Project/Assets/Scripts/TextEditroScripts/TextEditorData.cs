using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEditorData : MonoBehaviour
{
    private int id;
    private string username;
    private string startingDirecotry;
    private string displayText;
    private string pathToTheSelectedFile;
    private string wokringDirectory;

    public delegate void DisplayTextChangedHandler(string newText);
    public event DisplayTextChangedHandler OnDisplayTextChanged;

    public delegate void SelectedFileChangedHandler(string newText);
    public event SelectedFileChangedHandler OnSelectedFileChanged;

        
    //the id of the editor
    public int Id
    {
        get { return id; }
        set { id = value; }
    }
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
}
