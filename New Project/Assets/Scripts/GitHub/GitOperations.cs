using LibGit2Sharp;
using Octokit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class GitOperations : MonoBehaviour
{
    public static string CloneRepository(string sourceUrl, string destinationPath)
    {
        var co = new CloneOptions();
        var (owner, repoName) = GitHelperMethods.GetOwnerAndRepo(sourceUrl);

        string token = GetAccessToken();
        User user = GitHubClientProvider.client.User.Current().Result;
        string username = user.Login;

        print(username);
        co.CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials { Username = username, Password=token };
        
        string repoPath = $"{destinationPath}/{repoName}";  
        Directory.CreateDirectory(repoPath);
        
        string clonedRepoPath = LibGit2Sharp.Repository.Clone(sourceUrl, repoPath, co);
        return clonedRepoPath;
    }
    private static string GetAccessToken()
    {
        try
        {
            string json = SaveSystem.Load("accessToken.txt", "/Saves/");
            var loadedJson = JsonUtility.FromJson<Oauth2AccessToken.SaveToken>(json);
            return loadedJson.accessToken;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}
