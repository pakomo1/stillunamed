using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class RepoButtonTemplate : MonoBehaviour
{
    [SerializeField] private GameObject buttonTemplate;

    public void CreateButton(string text, string description, string profilePicUrl, bool visibility)
    {
        var button = Instantiate(buttonTemplate, transform);

        button.SetActive(true);
        button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
        button.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = description;

        StartCoroutine(GetImage(profilePicUrl, button.transform.GetChild(2).GetChild(0).GetComponent<Image>()));
        if (!visibility)
        {
            button.transform.GetChild(3).GetComponent<Image>().color = Color.green;
        }
        else if (visibility)
        {
            button.transform.GetChild(3).GetComponent<Image>().color = Color.red;
        }
    }

    private IEnumerator GetImage(string url, Image img)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            print("Error: " + www.error);
        }
        else
        {
            DownloadHandlerTexture textureDownloadHandler = (DownloadHandlerTexture)www.downloadHandler;
            Texture2D texture = textureDownloadHandler.texture;
            Sprite image = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
            img.sprite = image;
        }
        /*WWW www = new WWW(url);
         while (!www.isDone)
         {
             await Task.Yield();
         }
         return Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));*/
    }
}