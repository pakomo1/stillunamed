using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Octokit;
using UnityEngine.UI;
using System.Net.Http;
using UnityEngine.EventSystems;

public class RepoContentNavigation : MonoBehaviour
{
    [SerializeField] private GameObject contentHolder;
    [SerializeField] private GameObject repoContentUI;
    [SerializeField] private GameObject sideBarPanel;
    [SerializeField] private GameObject createOrUploadUi;
    [SerializeField] private GameObject branchContent;
    [SerializeField] private GameObject loadingContentScreen;
    [SerializeField] private Image repoOwenerProfilePicture;

    [SerializeField] private Button addfileButton;
    [SerializeField] private Button branchButtonTemplate;
    public Repository currentRepository;

    [SerializeField] private ValidAccessToken validAccessToken;
    [SerializeField] private RepoFilesTemplate repoFilesTemplate;
    [SerializeField] private branchesTemplate branchesTemplate;
    [SerializeField] private SingleActiveChild singleActiveChild;
    [SerializeField] private FilesContentNavigation filesContentNavigation;

    private event Action OnRepoContentLoadingStarted;
    private event Action OnRepoContentLoaded;
    private void Awake()
    {
        addfileButton.onClick.AddListener(() =>
        {
            createOrUploadUi.SetActive(!createOrUploadUi.activeSelf);
        });
        OnRepoContentLoadingStarted += RepoContentNavigation_OnRepoContentLoadingStarted;
        OnRepoContentLoaded += RepoContentNavigation_OnRepoContentLoaded;
    }

    private void RepoContentNavigation_OnRepoContentLoaded()
    {
        loadingContentScreen.SetActive(false);
    }

    private void RepoContentNavigation_OnRepoContentLoadingStarted()
    {
        loadingContentScreen.SetActive(true);
    }

    public async void ShowRepositoryContent(string repoOwner, string repoName, string path, string branch = "main")
    {
        try
        {
            OnRepoContentLoadingStarted?.Invoke();
            var repository = await GitHubClientProvider.client.Repository.Get(repoOwner, repoName);
            var repoContent = await GetRepositoryFiles.GetRepoFiles(repoOwner, repoName, path, branch);
            var repoBranches = await GetRepoBranches.GetBranches(repoOwner, repoName);

            currentRepository = repository;

            branchesTemplate.GenerateBranchs(repoBranches, currentRepository); 

            //ActivateObjectInContent.OnClickSwitchToThisUI(contentHolder, repoContentUI);

            singleActiveChild.ActivateChild(1);
            UpdateSideBarPanel(repository);
            await UpdateRepositoryContentUI(repository, repoContent, branch);
            OnRepoContentLoaded?.Invoke();
        }
        catch (HttpRequestException ex)
        {
            print(ex);
        }

    }
    private void UpdateSideBarPanel(Repository repoData)
    {
        var description = sideBarPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;
        var stars = sideBarPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text;
        var watching = sideBarPanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text;
        var forks = sideBarPanel.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text;

        description = String.IsNullOrEmpty(repoData.Description) ? "There is no description to this repo" : repoData.Description;
        sideBarPanel.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = description;
        sideBarPanel.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = repoData.StargazersCount.ToString();
        sideBarPanel.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = repoData.SubscribersCount.ToString();
        sideBarPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = repoData.ForksCount.ToString();
    }

    private async Task UpdateRepositoryContentUI(Repository repoData, IReadOnlyCollection<RepositoryContent> repoContents, string currentBranch)
    {
        branchButtonTemplate.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentBranch;

        await SetProfilePicture(repoData.Owner.Login);
        repoContentUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = repoData.Name;
        filesContentNavigation.Breadcrumb.Clear();
        repoFilesTemplate.GenerateRepoFiles(repoContents, repoData, currentBranch, filesContentNavigation.Breadcrumb);
    }
    private async Task SetProfilePicture(string username)
    {
        var user = await GitHubClientProvider.client.User.Get(username);
        using (var httpClient = new HttpClient())
        {
            var imageData = await httpClient.GetByteArrayAsync(user.AvatarUrl);

            var texture = new Texture2D(2, 2);
            texture.LoadImage(imageData); // This method automatically resizes the texture dimensions

            // Create a new Sprite using the Texture2D
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            // Assign the Sprite to the Image component
            repoOwenerProfilePicture.sprite = sprite;
        }
    }
}
