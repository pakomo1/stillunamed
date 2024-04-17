using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class TextEditorData : NetworkBehaviour
{
    private NetworkVariable<int> id = new NetworkVariable<int>();
    private NetworkVariable<FixedString128Bytes> username = new NetworkVariable<FixedString128Bytes>();
    private NetworkVariable<FixedString128Bytes> startingDirecotry = new NetworkVariable<FixedString128Bytes>("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private NetworkVariable<FixedString4096Bytes> displayText = new NetworkVariable<FixedString4096Bytes>("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private NetworkVariable<FixedString128Bytes> pathToTheSelectedFile = new NetworkVariable<FixedString128Bytes>("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private NetworkVariable<FixedString128Bytes> workingDirecotry = new NetworkVariable<FixedString128Bytes>("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    public delegate void DisplayTextChangedHandler(FixedString128Bytes newText);
    public event DisplayTextChangedHandler OnDisplayTextChanged;

    public delegate void SelectedFileChangedHandler(FixedString128Bytes newText);
    public event SelectedFileChangedHandler OnSelectedFileChanged;


    //the id of the editor
    /// <summary>
    /// Gets or sets the ID of the editor.
    /// </summary>
    public int Id
    {
        get { return id.Value; }
        set { id.Value = value; }
    }

    /// <summary>
    /// Gets or sets the directory in which we are currently in.
    /// </summary>
    public FixedString128Bytes WorkingDirectory
    {
        get { return workingDirecotry.Value; }
        set { workingDirecotry.Value = value; }
    }

    /// <summary>
    /// Gets or sets the path to the exe file of the selected file.
    /// </summary>
    public FixedString128Bytes StartingDirecotry
    {
        get { return startingDirecotry.Value; }
        set { startingDirecotry.Value = value; }
    }

    /// <summary>
    /// Gets or sets the username of the owner of this editor.
    /// </summary>
    public FixedString128Bytes OwnerUsername
    {
        get { return username.Value; }
        set { username.Value = value; }
    }

    /// <summary>
    /// Gets or sets the text that is supposed to be displayed in the input field.
    /// </summary>
    public FixedString4096Bytes DisplayText
    {
        get { return displayText.Value; }
        set { displayText.Value = value; }
    }
   
    /// <summary>
    /// Gets or sets the path to the selected file.
    /// </summary>
    public FixedString128Bytes PathToTheSelectedFile
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
    //sets the owner of the editor
    public void SetOwner(string owner)
    {
        OwnerUsername = owner;
    }
    //initialize path variables
    public void InitializePaths(string dir)
    {
        WorkingDirectory = dir;
        StartingDirecotry = dir;
    }
    //checks if is the owner
    public bool IsEditorDataOwner(string username)
    {
        return OwnerUsername == username;
    }
}
