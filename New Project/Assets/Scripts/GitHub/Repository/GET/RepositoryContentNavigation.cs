using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Octokit;

public class RepositoryContentNavigation : MonoBehaviour
{
    [SerializeField] private ValidAccessToken validAccessToken;
    [SerializeField] private GameObject contentHolder;
    [SerializeField] private GameObject repoContentUI;
    [SerializeField] private GameObject sideBarPanel;
    [SerializeField] private RepoFilesTemplate repoFilesTemplate;


    public async void ShowRepositoryContent(string repoOwner, string repoName, string path)
    {
        try
        {
            var repository = await GitHubClientProvider.client.Repository.Get(repoOwner, repoName);
            var repoContent = await GetRepositoryFiles.GetRepoFiles(repoOwner,repoName, path);
            var repoBranches = await GetRepoBranches.GetBranches(repoOwner, repoName);

            foreach (var branch in repoBranches)
            {
                print(branch.Name);
            }

            ActivateObjectInContent.OnClickSwitchToThisUI(contentHolder, repoContentUI);
            UpdateSideBarPanel(repository);
            UpdateRepositoryContentUI(repository, repoContent);
        }catch (Exception ex)
        {
            print(ex.Message);
        }

    }
    private void UpdateSideBarPanel(Repository repoData)
    {
        var description = sideBarPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;
        var stars = sideBarPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text;
        var watching = sideBarPanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text;
        var forks = sideBarPanel.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text;

        description = String.IsNullOrEmpty(repoData.Description)? "There is no description to this repo" : repoData.Description;
        sideBarPanel.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = description;
        sideBarPanel.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = repoData.StargazersCount.ToString();
        sideBarPanel.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = repoData.SubscribersCount.ToString();
        sideBarPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = repoData.ForksCount.ToString();
    }

    private void UpdateRepositoryContentUI(Repository repoData, IReadOnlyCollection<RepositoryContent> repoContents)
    {
        repoContentUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = repoData.Name;
        print(repoData.Name);
        repoFilesTemplate.GenerateRepoFiles(repoContents,repoData);
    }
}
