using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TextEditorData : NetworkBehaviour
{
    private NetworkVariable<int> id = new NetworkVariable<int>();
    private NetworkVariable<string> username = new NetworkVariable<string>();
    private NetworkVariable<string> startingDirecotry = new NetworkVariable<string>();
    private NetworkVariable<string> displayText = new NetworkVariable<string>();
    private NetworkVariable<string> pathToTheSelectedFile = new NetworkVariable<string>();
    private NetworkVariable<string> workingDirecotry = new NetworkVariable<string>();


    public delegate void DisplayTextChangedHandler(string newText);
    public event DisplayTextChangedHandler OnDisplayTextChanged;

    public delegate void SelectedFileChangedHandler(string newText);
    public event SelectedFileChangedHandler OnSelectedFileChanged;

        
    //the id of the editor
    public int Id
    {
        get { return id.Value; }
        set
        {
            id.Value = value;
        }
    }
    // the directroy we in which we are currently in
    public string WorkingDirectory
    {
        get { return workingDirecotry.Value; }
        set
        {
             workingDirecotry.Value = value;
        }
    }
    //the path to the exe file of the selected file
    public string StartingDirecotry
    {
        get { return startingDirecotry.Value; }
        set
        {
            startingDirecotry.Value = value;
        }
    }
    //the user that this editor belongs to
    public string OwnerUsername
    {
        get { return username.Value; }
        set
        {
            username.Value = value;
        }
    }
    //the text that is supposed to be displayed in the inputField
    public string DisplayText
    {
        get { return displayText.Value; }
        set        
        {
             displayText.Value = value;
        }

    }
    //the path to the selected file
    public string PathToTheSelectedFile
    {
        get { return pathToTheSelectedFile.Value; }
        set
        {
                if (pathToTheSelectedFile.Value != value)
                {
                    pathToTheSelectedFile.Value = value;
                    // Trigger the event
                    OnSelectedFileChanged?.Invoke(pathToTheSelectedFile.Value);
                }
        }
    }
}
