using System;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Octokit;
using System;
using UnityEditor;

public class ValidAccessToken : MonoBehaviour
{
    [SerializeField] private OpenServer openServer;
    [SerializeField] private GetUserRepoInfo repoInfo;
    [SerializeField] private DataBaseManager dbManager;
    private string accessToken;
    private string userReposURL = "https://api.github.com/user/repos";

    private void Start()
    {
        ValidateToken();
    }

    public async void ValidateToken()
    {
        try
        {
            accessToken = GetAccessToken();
            GitHubClientProvider.GetGitHubClient(accessToken);
            var client = GitHubClientProvider.client;

            User user = await client.User.Current();
            var repositories = await client.Repository.GetAllForCurrent();

            //register a user if not registered
            bool userExists = await dbManager.CheckIfUserExists(user.Login);
            if (userExists)
            {
                print("The user exists");
            }
            else
            {
                print("Saving the user");
                dbManager.SaveUser(user.Login);
            }
            openServer.authorized = true;
            repoInfo.repositories = repositories;
            repoInfo.GenerateButtons();
        }
        catch (AuthorizationException)
        {
            openServer.authorized = false;
            print("Invalid access token. Please log in again.");
        }
        catch (Exception ex)
        {
            openServer.authorized = false;
            print("An error occurred: " + ex);
        }
    }
    public string GetAccessToken()
    {
        try
        {
            string json = SaveSystem.Load("accessToken.txt", "/Saves/");
            var loadedJson = JsonUtility.FromJson<AccessTokenResponse>(json);
            return loadedJson.access_token;
        }catch(Exception ex)
        {
            return ex.Message;
        }
    }
}
