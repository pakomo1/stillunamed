using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneMetadata : MonoBehaviour
{
    private static string githubRepoLink;
    private static string githubRepoPath;
    private static string currentBranch = "main";
    public static event Action OnBranchChanged;

    public static string GithubRepoLink { get => githubRepoLink; set => githubRepoLink = value; }
    public static string GithubRepoPath { get => githubRepoPath; set => githubRepoPath = value; }
    public static string CurrentBranch
    {
           get => currentBranch; 
        set
        {
            currentBranch = value;
            OnBranchChanged?.Invoke();
        }
    }

    //gets the owner and repo name from the github link and returns them as a string array
    public static string[] GetOwnerAndRepoName()
    {
        var uri = new System.Uri(GithubRepoLink);
        var segments = uri.AbsolutePath.Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries);
        string owner = segments[0];
        string repo = segments[1];
        return new string[] { owner, repo };
    }


}
