using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Octokit;

public class RepoButtonTemplate : MonoBehaviour
{
    [SerializeField] private GameObject buttonTemplate;
    [SerializeField] private RepositoryContentNavigation repoContentNavigation;

    public async void CreateButton(string repoName, string description, string profilePicUrl, bool visibility, string repoOwner)
    {
        //Repository repo = await GitHubClientProvider.client.Repository.Get(repoOwner, repoName);

        var button = Instantiate(buttonTemplate, transform);
        button.SetActive(true);

        TextMeshProUGUI repoNameTextMesh = button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        repoNameTextMesh.overflowMode = TextOverflowModes.Ellipsis;
        repoNameTextMesh.text = repoName;

        TextMeshProUGUI descriptionTextMesh = button.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        descriptionTextMesh.overflowMode = TextOverflowModes.Ellipsis;
        descriptionTextMesh.text = description;

        Sprite image = await GetImage(profilePicUrl);
        button.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = image;
        if (!visibility)
        {
            button.transform.GetChild(3).GetComponent<Image>().color = Color.green;
        }
        else if (visibility)
        {
            button.transform.GetChild(3).GetComponent<Image>().color = Color.red;
        }
        button.GetComponent<Button>().onClick.AddListener(() => repoContentNavigation.ShowRepositoryContent(repoOwner, repoName, "/"));
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