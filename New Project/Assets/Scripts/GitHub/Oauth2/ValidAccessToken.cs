using System;
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
    private string userReposURL = "https://api.github.com/user/repos";

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
        

            openServer.authorized = true;
            repoInfo.repositories = repositories;
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
        try
        {
            string json = SaveSystem.Load("accessToken.txt", "/Saves/");
            var loadedJson = JsonUtility.FromJson<Oauth2AccessToken.SaveToken>(json);
            return loadedJson.accessToken;
        }catch(Exception ex)
        {
            return ex.Message;
        }
    }
}
