using Octokit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using static Oauth2AccessToken;

public class DeviceFlowToken : MonoBehaviour
{
    [SerializeField] private GameObject uiAfterCompletion;
    [SerializeField] private GameObject uiBeforeCompletion;

    [SerializeField] private GameObject AuthenticateInfoUI;
    [SerializeField] private TextMeshProUGUI url;
    [SerializeField] private TextMeshProUGUI usercode;


    private string deviceCodeEndpoint = "https://github.com/login/device/code";
    private string tokenEndpoint = "https://github.com/login/oauth/access_token";
    private string clientId = "e4edd56045a448fbdc0f";
    private string deviceCode;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //1.Device Code Request
    public async void GetDeviceCode()
    {
        var client = new GitHubClient(new ProductHeaderValue("MyApp"));
        var request = new OauthDeviceFlowRequest(clientId);
        request.Scopes.Add("repo");

        var response = await client.Oauth.InitiateDeviceFlow(request);

        if (response != null)
        {
            // Display the user code and verification URL to the user
            Debug.Log("Please go to " + response.VerificationUri + " and enter this code: " + response.UserCode);
            AuthenticateInfoUI.SetActive(true);
            url.text = response.VerificationUri;
            usercode.text = response.UserCode;


            // Store the device code for later
            deviceCode = response.DeviceCode;
            StartCoroutine(PollForAccessToken());
        }
        else
        {
            Debug.Log("Error: " + response);
        }
    }
    //3.Polling for Access Token
    private IEnumerator PollForAccessToken()
    {
        WWWForm form = new WWWForm();
        form.AddField("client_id", clientId);
        form.AddField("device_code", deviceCode);
        form.AddField("grant_type", "urn:ietf:params:oauth:grant-type:device_code");

        while (true)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(tokenEndpoint, form))
            {
                // Set the Accept header to application/json
                www.SetRequestHeader("Accept", "application/json");

                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    // Parse the server's response
                    var parsedResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(www.downloadHandler.text);

                    // Check if the response contains an error
                    if (parsedResponse.ContainsKey("error"))
                    {
                        // The user has not yet completed the device authorization flow, continue polling
                        Debug.Log("Polling...");
                    }
                    else
                    {
                        // The user has completed the device authorization flow, use the access token
                        string accessToken = parsedResponse["access_token"];
                        string tokenType = parsedResponse["token_type"];
                        string scope = parsedResponse["scope"];

                        print("Your access token: " + accessToken);
                        AssignToken(accessToken, tokenType, scope);

                        // Stop polling
                        yield break;
                    }
                }
                else
                {
                    // If the response is not a 400 error, something went wrong
                    Debug.Log(www.error);
                    yield break;
                }
            }
            // Wait before polling again
            yield return new WaitForSeconds(5);
        }
    }
    private void AssignToken(string result,string token_type, string scope)
    {
        //Initialize the save system
        SaveSystem.INIT("/Saves/");

        //Turn the access token info into a json format
        AccessTokenResponse saveToken = new AccessTokenResponse() { access_token = result, token_type=token_type, scope= scope };
        string json = JsonUtility.ToJson(saveToken);
        SaveSystem.Save(json, "/accessToken.txt", "/Saves/");

        //UI panel switch
        AuthenticateInfoUI.SetActive(false);
        uiAfterCompletion.SetActive(true);
    }
}

[Serializable]
public class DeviceCodeResponse
{
    public string device_code;
    public string user_code;
    public string verification_uri;
    public int expires_in;
    public int interval;
}

[Serializable]
public class AccessTokenResponse
{
    public string access_token;
    public string token_type;
    public string scope;
}
