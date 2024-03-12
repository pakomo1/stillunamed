using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using Octokit;
using Cysharp.Threading.Tasks;

public class Oauth2AccessToken : MonoBehaviour
{
    [SerializeField] private GameObject uiAfterCompletion;
    [SerializeField] private GameObject uiBeforeCompletion;
    private string code;
    private string clientId = "e4edd56045a448fbdc0f";
    private string clientSecret = "8e706e8c6be08f9ff09ba1493261f0eeb0dd1522";
    private string result;
    private string[] prefixes = new string[1] { "https://eog0ubb2uup5m2r.m.pipedream.net" };
    private GameObject playerObjectPrefab;

    public async void AuthorizeBegin()
    {
        HttpListener listener = new HttpListener();
        foreach (string s in prefixes)
        {       
            listener.Prefixes.Add(s);
        }
        listener.Start();
        print("Listening...");

        // Note: The GetContext method blocks while waiting for a request.
        HttpListenerContext context = listener.GetContext();
        HttpListenerRequest request = context.Request;

        // Obtain a response object.
        HttpListenerResponse response = context.Response;

        // Construct a response.
        string responseString = SaveSystem.Load("/main.txt", "/WebPage/");
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

        // Get a response stream and write the response to it.
        response.ContentLength64 = buffer.Length;
        System.IO.Stream output = response.OutputStream;
        output.Write(buffer, 0, buffer.Length);

        // Close the output stream.
        output.Close();
        listener.Stop();

        // Get the query string from the URL
        string queryString = request.Url.Query;

        // Remove the leading '?' from the query string
        if (!string.IsNullOrEmpty(queryString) && queryString.StartsWith("?"))
        {
            queryString = queryString.Substring(1);
        }

        // Split the query string into parameters
        string[] parameters = queryString.Split('&');

        // Find the 'code' parameter
        foreach (string parameter in parameters)
        {
            string[] parts = parameter.Split('=');
            if (parts.Length == 2 && parts[0] == "code")
            {
                code = parts[1];
                break;
            }
        }
        print("code: " + code);
        // Get Access Token with code
        result = await Authorize();
        print(result);

        AssignToken();
    }
    public async UniTask<string> Authorize()
    {
        var request = new OauthTokenRequest(clientId, clientSecret, code);
        var token = await GitHubClientProvider.client.Oauth.CreateAccessToken(request);
        GitHubClientProvider.GetGitHubClient(token.AccessToken);
        return token.AccessToken;
        }

    private void AssignToken()
    {
        // Assign Access Token to Player
        //playerObjectPrefab = GameObject.FindWithTag("Player");
        //playerObjectPrefab.GetComponentInChildren<AccessToken>().accessToken = result;

        //Initialize the save system
        SaveSystem.INIT("/Saves/");
        
        //Turn the access token info into a json format
        SaveToken saveToken = new SaveToken() { accessToken = result };
        string json = JsonUtility.ToJson(saveToken);
        SaveSystem.Save(json, "/accessToken.txt", "/Saves/");

        //UI panel switch
        uiBeforeCompletion.SetActive(false);
        uiAfterCompletion.SetActive(true);
    }

    public class SaveToken
    {
        public string accessToken;
    }
}
