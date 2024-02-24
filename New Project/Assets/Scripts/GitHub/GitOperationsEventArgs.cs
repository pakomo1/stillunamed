using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GitOperationsEventArgs : EventArgs
{
    public string ErrorMessage { get; }

    public GitOperationsEventArgs(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}
