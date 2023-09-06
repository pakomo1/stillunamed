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
    private string clientId = "086490aaf0f9ef0b33e4";
    private string clientSecret = "f1fe8180ea2712c9ce2a282a035799e9f2129093";
    private GitHubClient client = new GitHubClient(new ProductHeaderValue("TestApp"));
    private string result;
    private string[] prefixes = new string[1] { "http://localhost:3000/callback/" };
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

        // Get code from URI
        string[] rawUri = request.RawUrl.Split('=');
        code = rawUri[1];
        print(code);

        // Get Access Token with code
        result = await Authorize();
        print(result);

        AssignToken();
    }
    public async UniTask<string> Authorize()
    {
        var request = new OauthTokenRequest(clientId, clientSecret, code);
        var token = await client.Oauth.CreateAccessToken(request);
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
