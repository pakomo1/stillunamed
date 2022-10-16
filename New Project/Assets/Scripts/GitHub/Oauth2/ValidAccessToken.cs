using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class ValidAccessToken : MonoBehaviour
{
    [SerializeField] private OpenServer openServer; 
    private HttpWebResponse httpResponse;
    private string accessToken;
    private string url = "https://api.github.com/user/repos";

    private void Awake()
    {
        //Get the access token from accessToken.txt
        string json = SaveSystem.Load("accessToken.txt");
        var loadedJson = JsonUtility.FromJson<Oauth2AccessToken.SaveToken>(json);
        accessToken = loadedJson.accessToken;

        ValidateToken();
    }

    private async void ValidateToken()
    {
        using var www = UnityWebRequest.Get(url);
        www.SetRequestHeader("Authorization", "Bearer " + accessToken);
        var operation = www.SendWebRequest();
        while (!operation.isDone)
            await Task.Yield();

        if (www.result == UnityWebRequest.Result.Success)
        {
            print(www.responseCode);
            if (www.responseCode == 200)
            {
                openServer.authorized = true;
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
}
