using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class TextEditor : MonoBehaviour
{
    private string userName;
    private string startingDirecotry;
    private string displayText;
    private string pathToTheSelectedFile;
    private string pathToSelectedExeFile;
    private string wokringDirectory;

    // the directroy we in which we are currently in
    public string WorkingDirectory
    {
        get { return wokringDirectory; }
        set { wokringDirectory = value; }
    }
    //the path to the exe file of the selected file
    public string PathToSelectedExeFile
    {
        get { return pathToSelectedExeFile; }
        set { pathToSelectedExeFile = value; }
    }
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
    public TextEditor()
    {
        DisplayText = "";
    }
}
