using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class RepoButtonTemplate : MonoBehaviour
{
    [SerializeField] private GameObject buttonTemplate;


    public void CreateButton(string text, string description, string profilePicUrl, bool visibility)
    {
        var button = Instantiate(buttonTemplate, transform);
        button.SetActive(true);
        button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
        button.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = description;
        //Sprite image = GetImage(profilePicUrl);
        //button.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = image;
        GetImage(profilePicUrl, button.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite);
        if (!visibility)
        {
            button.transform.GetChild(3).GetComponent<Image>().color = Color.green;
        }
        else if(visibility)
        {
            button.transform.GetChild(3).GetComponent<Image>().color = Color.red;
        }
    }

     private IEnumerator GetImage(string url, Sprite img)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            print(request.error);
        }
        else
        {
            Texture2D myTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite image = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0, 0));
            img = image;
        }


        /*WWW www = new WWW(url);
         while (!www.isDone)
        {
            await Task.Yield();
        }
        return Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));*/
    }


}
