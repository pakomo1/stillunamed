using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class RepositoryContentNavigation : MonoBehaviour
{
    [SerializeField] private ValidAccessToken validAccessToken;
    [SerializeField] private GameObject contentHolder;
    [SerializeField] private GameObject repoContentUI;
    [SerializeField] private GameObject sideBarPanel;
    [SerializeField] private GetRepositoryFiles getRepositoryFiles;
    [SerializeField] private RepoContentTemplate repoContentTemplate;

    public async void ShowRepositoryContent(string repoOwner, string repoName, string path)
    {
        //https://api.github.com/repos/{repoOwner}/{repoName}/contents/{path}
        string url = $"https://api.github.com/repos/{repoOwner}/{repoName}";

        var accessToken = validAccessToken.GetAccessToken();
        using var request = UnityWebRequest.Get(url);

        request.SetRequestHeader("Authorization", "Bearer " + accessToken);
        var operation = request.SendWebRequest();
        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (request.result == UnityWebRequest.Result.Success)
        {
            print("Successfully fetched repository information" + request.responseCode);
            if (request.responseCode == 200)
            {
                var repoContent = await getRepositoryFiles.GetRepoFiles(repoName,repoOwner, path);
                var deserializedData = JsonUtility.FromJson<RepositoryData>(request.downloadHandler.text);

                ActivateObjectInContent.OnClickSwitchToThisUI(contentHolder, repoContentUI);
                UpdateSideBarPanel(deserializedData);
                UpdateRepositoryContentUI(deserializedData,repoContent);

            }
            else
            {
                print("Could not complete task");
            }
        }
        else
        {
            Debug.Log("Error" + request.error);
        }
    }

    private void UpdateSideBarPanel(RepositoryData repoData)
    {
        var description = sideBarPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;
        var stars = sideBarPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text;
        var watching = sideBarPanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text;
        var forks = sideBarPanel.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text;

        repoData.description = String.IsNullOrEmpty(repoData.description)? "There is no description to this repo" : repoData.description;
        sideBarPanel.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = repoData.description;
        sideBarPanel.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = repoData.stargazers_count.ToString();
        sideBarPanel.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = repoData.watchers.ToString();
        sideBarPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = repoData.forks_count.ToString();
    }

    private void UpdateRepositoryContentUI(RepositoryData repoData, List<GetRepositoryFiles.RepoContent> repoContents)
    {
        repoContentUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = repoData.name;
        repoContentTemplate.GenerateRepoFiles(repoContents);
    }
    [Serializable]
    public class RepositoryData
    {
        public string name;
        public string description;
        public string contents_url;
        public string commits_url;
        public int forks_count;
        public int watchers;
        public int stargazers_count;
    }
}
