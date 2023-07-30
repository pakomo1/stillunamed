using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ApiRequestHelper
{
    [SerializeField] private ValidAccessToken validAccessToken;
    public UnityWebRequest CreateAuthRequest(string url, RequestType type = RequestType.GET , object data = null)
    {
        UnityWebRequest request = new UnityWebRequest(url,type.ToString());
        var accessToken = validAccessToken.GetAccessToken();
        request.SetRequestHeader("Authorization", "Bearer " + accessToken);

        if (data != null)
        {
            var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        }

        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        return request;
    }


    public enum RequestType
    {
        GET = 0,
        POST = 1,
        PUT = 2
    }
}
