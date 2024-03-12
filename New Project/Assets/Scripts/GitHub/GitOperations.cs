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
    public static async Task<string> CloneRepositoryAsync(string sourceUrl, string destinationPath)
    {
        var co = new CloneOptions();

        string token = GetAccessToken();
        User user = await GitHubClientProvider.client.User.Current();
        string username = user.Login;

        print("Username" + username);
        print("Token: " +token);

        co.CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials { Username = username, Password = token };

        Directory.CreateDirectory(destinationPath);

        string clonedRepoPath = await Task.Run(() => LibGit2Sharp.Repository.Clone(sourceUrl, destinationPath, co));
        return clonedRepoPath;
    }
    private static string GetAccessToken()
    {
        try
        {
            string json = SaveSystem.Load("accessToken.txt", "/Saves/");
            var loadedJson = JsonUtility.FromJson<AccessTokenResponse>(json);
            return loadedJson.access_token;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}
