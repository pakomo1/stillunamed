using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft;
using System;
using Octokit;

public class RepostoryViewerManager : MonoBehaviour
{
    [SerializeField] private Image repoOwenerProfilePicture;
    [SerializeField] private TextMeshProUGUI repoOwnerName;
    [SerializeField] private TextMeshProUGUI repositoryNameLbl;
    [SerializeField] private RepoFilesTemplate repoFilesTemplate;
    [SerializeField] private FilesContentNavigation filesContentNavigation;
    [SerializeField] private SingleActiveChild singleActiveChild;
    public string currentBranch = "main";

    [SerializeField] private Button codeButton;
    [SerializeField] private Button issuesButton;
    [SerializeField] private Button CommitsButton;

    [SerializeField] private CommitButtonTemplate commitButtonTemplate;

    private string owner;
    private string repo;

    private void Start()
    {
        string[] ownerandrepo = GameSceneMetadata.GetOwnerAndRepoName();
        owner = ownerandrepo[0];
        repo = ownerandrepo[1];
        SetProfilePicture(owner);
        repoOwnerName.text = owner;

        singleActiveChild.ActivateOneChild(0);
        GenerateRepoFiles(owner, repo);

        codeButton.onClick.AddListener(() =>
        {
            singleActiveChild.ActivateOneChild(0);
            GenerateRepoFiles(owner, repo);
        });

        issuesButton.onClick.AddListener(() =>
        {

        });

        CommitsButton.onClick.AddListener(() =>
        {
            singleActiveChild.ActivateOneChild(1);
            GenerateCommits(owner, repo);
        });
        GameSceneMetadata.OnBranchChanged += GameSceneMetadata_OnBranchChanged; 
    }

    private void GameSceneMetadata_OnBranchChanged()
    {
        print(GameSceneMetadata.CurrentBranch);
    }

    private async void SetProfilePicture(string username)
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
    private async void GenerateRepoFiles(string owner, string repoName)
    {
        var contents = await GitHubClientProvider.client.Repository.Content.GetAllContents(owner, repoName);
        var repository = await GitHubClientProvider.client.Repository.Get(owner, repoName);

        filesContentNavigation.Breadcrumb.Clear(); // Clear the breadcrumb when generating the root files
        repoFilesTemplate.GenerateRepoFiles(contents, repository, currentBranch, filesContentNavigation.Breadcrumb);
    }
    //gets the commits of the repository and generates them
    private async void GenerateCommits(string owner, string repoName)
    {
        var request = new CommitRequest { Sha = currentBranch };
        var commits = await GitHubClientProvider.client.Repository.Commit.GetAll(owner, repoName, request);

        commitButtonTemplate.CreateButtonsForCommits(commits);
    }

}