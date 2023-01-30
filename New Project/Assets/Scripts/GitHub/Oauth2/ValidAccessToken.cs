using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

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
        accessToken = GetAccessToken();
        using var www = UnityWebRequest.Get(url);
        www.SetRequestHeader("Authorization", "Bearer " + accessToken);
        var operation = www.SendWebRequest();
        while (!operation.isDone)
            await Task.Yield();

        if (www.result == UnityWebRequest.Result.Success)
        {
            print("Access Token validation response code " + www.responseCode);
            if (www.responseCode == 200)
            {
                openServer.authorized = true;
                repoInfo.json = www.downloadHandler.text;
                repoInfo.GenerateButtons();
            }
            else
            {
                openServer.authorized = false;
            }
        }
        else
        {
            Debug.Log("Error" + www.error);
        }      
    }
    public string GetAccessToken()
    {
        string json = SaveSystem.Load("accessToken.txt", "/Saves/");
        var loadedJson = JsonUtility.FromJson<Oauth2AccessToken.SaveToken>(json);
        return loadedJson.accessToken;
    }
}
