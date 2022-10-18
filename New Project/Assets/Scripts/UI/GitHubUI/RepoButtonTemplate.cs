using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RepoButtonTemplate : MonoBehaviour
{
    [SerializeField] private GameObject buttonTemplate;

    private void Awake()
    {
        createButton("test", "desc", "https://www.industrialempathy.com/img/remote/ZiClJf-1920w.jpg", false);
        createButton("test2", "desc2", "https://www.industrialempathy.com/img/remote/ZiClJf-1920w.jpg", true);
    }

    private async void createButton(string text, string description, string profilePicUrl, bool visibility)
    {
        var button = Instantiate(buttonTemplate, transform);
        button.SetActive(true);
        button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
        button.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = description;
        Sprite image = await GetImage(profilePicUrl);
        button.transform.GetChild(2).GetComponent<Image>().sprite = image;
        if (visibility)
        {
            button.transform.GetChild(3).GetComponent<Image>().color = Color.green;
        }
        else if(!visibility)
        {
            button.transform.GetChild(3).GetComponent<Image>().color = Color.red;
        }
    }

    private async Task<Sprite> GetImage(string url)
    {
        WWW www = new WWW(url);
        while (!www.isDone)
        {
            await Task.Yield();
        }
        return Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
    }
}
