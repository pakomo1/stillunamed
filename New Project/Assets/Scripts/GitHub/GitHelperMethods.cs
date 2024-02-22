using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GitHelperMethods : MonoBehaviour
{
    public static (string owner, string repoName) GetOwnerAndRepo(string repoUrl)
    {
        Uri repoUri = new Uri(repoUrl);

        if (repoUri.Segments.Length >= 3)
        {
            string owner = repoUri.Segments[1].TrimEnd('/');
            string repoName = repoUri.Segments[2].TrimEnd('/');
            repoName = repoName.Substring(0, repoName.Length - 4);

            return (owner, repoName);
        }
        throw new ArgumentException("Invalid GitHub repository URL");
    }
}
