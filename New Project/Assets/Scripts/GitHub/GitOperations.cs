using LibGit2Sharp;
using Octokit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        print("Username: " + username);
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
    //check if the user is the owner of the repository
    public static async Task<bool> IsUserRepoOwnerAsync(string username, string repoLink)
    {
        var url = new Uri(repoLink);
        var repoName = url.Segments.Last();
        var ownerName = url.Segments[url.Segments.Length - 2].Trim('/');

        var repo = await GitHubClientProvider.client.Repository.Get(ownerName, repoName);
        return repo.Owner.Login == username;
    }
    // Check if the user is a collaborator of the repository
    public static async Task<bool> IsUserCollaboratorAsync(string username, string repoLink)
    {
        var url = new Uri(repoLink);
        var repoName = url.Segments.Last();
        var ownerName = url.Segments[url.Segments.Length - 2].Trim('/');

        var collaborators = await GitHubClientProvider.client.Repository.Collaborator.GetAll(ownerName, repoName);
        return collaborators.Any(c => c.Login == username);
    }
    //invites a user to a repository
    public static async Task InviteUserToRepoAsync(string username, string repoLink)
    {
        var url = new Uri(repoLink);
        var repoName = url.Segments.Last();
        var ownerName = url.Segments[url.Segments.Length - 2].Trim('/');

        await GitHubClientProvider.client.Repository.Collaborator.Invite(ownerName, repoName, username);
    }
}
