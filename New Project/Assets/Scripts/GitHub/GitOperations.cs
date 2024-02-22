using LibGit2Sharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GitOperations : MonoBehaviour
{
    public static void CloneRepository(string sourceUrl, string destinationPath)
    {
        Repository.Clone(sourceUrl, destinationPath);
    }
}
