using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Octokit;
using System;

public class ValidAccessToken : MonoBehaviour
{
    [SerializeField] private OpenServer openServer;
    [SerializeField] private GetUserRepoInfo repoInfo;
    private string accessToken;
    private string url = "https://api.github.com/user/repos";

    private void Awake()
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
        

            repoInfo.repositories = repositories;
            openServer.authorized = true;
            repoInfo.GenerateButtons();
        }
        catch (Exception ex)
        {
            openServer.authorized = false;
            print(ex.Message);
        }
    }
    public string GetAccessToken()
    {
        string json = SaveSystem.Load("accessToken.txt", "/Saves/");
        var loadedJson = JsonUtility.FromJson<Oauth2AccessToken.SaveToken>(json);
        return loadedJson.accessToken;
    }
}
