using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEditor 
{
    private string userName;
    private string startingDirecotry;

    public string StartingDirecotry
    {
        get { return startingDirecotry; }
        set { startingDirecotry = value; }
    }
    public string UserName
    {
        get { return userName; }
        set { userName = value; }
    }
    public TextEditor(string userName, string startingDirecotry)
    {
        UserName = userName;
        StartingDirecotry = startingDirecotry;
        
    }
}
