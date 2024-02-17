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
    [SerializeField] private GameObject topicsContentHolder;
    [SerializeField] private Image userImage;
    public async void CreateButton(IReadOnlyList<Repository> repositories)
    {
        foreach ( var repo in repositories)
        {
            string name = repo.Name;
            string avatarUrl = repo.Owner.AvatarUrl;
            string mostUsedLanguage = repo.Language;
            int stars = repo.StargazersCount;
            DateTimeOffset lastUpdate = repo.UpdatedAt;

            var readme = await GitHubClientProvider.client.Repository.Content.GetReadme(repo.Id);
            string readmeContent = readme.Content;
            string title = readmeContent.Split('\n')[0].Replace("# ", "");

            var topics = await GitHubClientProvider.client.Repository.GetAllTopics(repo.Id);
            CreateTopicButtonTemplate(topics);

            var button = Instantiate(buttonTemplate, transform);
            button.gameObject.SetActive(true);

            var footer = button.transform.Find("Footer");   

            button.transform.Find("repositoryName").GetComponent<TextMeshProUGUI>().text = name;
            button.transform.Find("descriptionTitle").GetComponent<TextMeshProUGUI>().text = title;

            footer.Find("mostusedLanguage").GetComponent<TextMeshProUGUI>().text = mostUsedLanguage;
            footer.Find("stars").GetComponent<TextMeshProUGUI>().text = stars.ToString();
            string formattedDate = lastUpdate.ToString("MMM dd, yyyy");
            footer.Find("lastUpdateDate").GetComponent<TextMeshProUGUI>().text = $"Updated on {formattedDate}";

            button.transform.Find("userProfilePicture").GetComponent<Image>().sprite = await GetImage(avatarUrl);
            button.onClick.AddListener(() => OnRepoButtonClick(repo));
        }
    }

    private void CreateTopicButtonTemplate(RepositoryTopics topics)
    {
        foreach (var topic in topics.Names)
        {
            var button = Instantiate(topicButtonTemplate, topicsContentHolder.transform);   
            button.gameObject.SetActive(true);
            button.transform.Find("topicName").GetComponent<TextMeshProUGUI>().text = topic;
        }
    }

    private void OnRepoButtonClick(Repository repo)
    {
        throw new NotImplementedException();
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
