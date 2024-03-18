using System.Collections;
using System.Collections.Generic;
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
    public int Id
    {
        get { return id.Value; }
        set
        {
            id.Value = value;
        }
    }
    // the directroy we in which we are currently in
    public FixedString128Bytes WorkingDirectory
    {
        get { return workingDirecotry.Value; }
        set
        {
             workingDirecotry.Value = value;
        }
    }
    //the path to the exe file of the selected file
    public FixedString128Bytes StartingDirecotry
    {
        get { return startingDirecotry.Value; }
        set
        {
            startingDirecotry.Value = value;
        }
    }
    //the user that this editor belongs to
    public FixedString128Bytes OwnerUsername
    {
        get { return username.Value; }
        set
        {
            username.Value = value;
        }
    }
    //the text that is supposed to be displayed in the inputField
    public FixedString4096Bytes DisplayText
    {
        get { return displayText.Value; }
        set        
        {
             displayText.Value = value;
        }

    }
    //the path to the selected file
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
}
