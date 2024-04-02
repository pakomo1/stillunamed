using Octokit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class SearchRepoButtonTemplate : MonoBehaviour
{
    [SerializeField]private Button buttonTemplate;
    [SerializeField]private Button topicButtonTemplate;
    [SerializeField]private GameObject topicsContentHolder;
    [SerializeField]private RepoContentNavigation repositoryContentNavigationManager;
    [SerializeField]private Image userImage;

    public event EventHandler OnFinishedGeneratingRepoButtons;
    public async void CreateButton(IReadOnlyList<Repository> repositories)
    {
        ClearContent();
        foreach ( var repo in repositories)
        {
            string name = repo.Name;
            string avatarUrl = repo.Owner.AvatarUrl;
            string mostUsedLanguage = repo.Language;
            int stars = repo.StargazersCount;
            DateTimeOffset lastUpdate = repo.UpdatedAt;

            string about = repo.Description;

            var topics = await GitHubClientProvider.client.Repository.GetAllTopics(repo.Id);

            var button = Instantiate(buttonTemplate, transform);
            button.gameObject.SetActive(true);

            //create topic buttons
            CreateTopicButtonTemplate(topics,button.transform.GetChild(2));

            var footer = button.transform.Find("Footer");
            var repositoryName = button.transform.Find("repositoryName");

            repositoryName.GetComponent<TextMeshProUGUI>().text = name;
            button.transform.Find("descriptionTitle").GetComponent<TextMeshProUGUI>().text = about;

            footer.Find("mostusedLanguage").GetComponent<TextMeshProUGUI>().text = mostUsedLanguage;
            footer.Find("stars").GetComponent<TextMeshProUGUI>().text = stars.ToString();
            string formattedDate = lastUpdate.ToString("MMM dd, yyyy");
            footer.Find("lastUpdateDate").GetComponent<TextMeshProUGUI>().text = $"Updated on {formattedDate}";

            repositoryName.transform.Find("userProfilePicture").GetComponent<Image>().sprite = await GetImage(avatarUrl);
            button.onClick.AddListener(() => OnRepoButtonClick(repo));
        }
        OnFinishedGeneratingRepoButtons?.Invoke(this, EventArgs.Empty);
    }

    private void CreateTopicButtonTemplate(RepositoryTopics topics, Transform parent)
    {
        foreach (var topic in topics.Names)
        {
            var button = Instantiate(topicButtonTemplate, parent);   
            button.gameObject.SetActive(true);
            button.transform.Find("topicName").GetComponent<TextMeshProUGUI>().text = topic;
        }
    }

    private void OnRepoButtonClick(Repository repo)
    {
        repositoryContentNavigationManager.ShowRepositoryContent(repo.Owner.Login, repo.Name, "\\");
    }
    //clear content
    public void ClearContent()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }


    private Task<Sprite> GetImage(string url)
    {
        var tcs = new TaskCompletionSource<Sprite>();
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        www.SendWebRequest().completed += operation =>
        {
            if (www.result != UnityWebRequest.Result.Success)
            {
                tcs.SetException(new Exception(www.error));
            }
            else
            {
                Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                var result = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                tcs.SetResult(result);
            }
        };

        return tcs.Task;
    }

}
