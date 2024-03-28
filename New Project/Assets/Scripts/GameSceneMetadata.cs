using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneMetadata : MonoBehaviour
{
    private static string githubRepoLink;
    private static string githubRepoPath;

    public static string GithubRepoLink { get => githubRepoLink; set => githubRepoLink = value; }
    public static string GithubRepoPath { get => githubRepoPath; set => githubRepoPath = value; }
}
