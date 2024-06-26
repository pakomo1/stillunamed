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
    [SerializeField] private IssueUIManager issueUIManager;
    public string currentBranch = "main";

    [SerializeField] private Button codeButton;
    [SerializeField] private Button issuesButton;
    [SerializeField] private Button CommitsButton;
    [SerializeField] private Button showViewerUI;

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
        repositoryNameLbl.text = repo;  

        singleActiveChild.ActivateOneChild(0);
        GenerateRepoFiles(owner, repo);

        codeButton.onClick.AddListener(() =>
        {
            singleActiveChild.ActivateOneChild(0);
            GenerateRepoFiles(owner, repo);
        });

        issuesButton.onClick.AddListener(() =>
        {
            //craets an issue request
            RepositoryIssueRequest issueRequest = new RepositoryIssueRequest()
            {
                Filter = IssueFilter.All,
            };
            singleActiveChild.ActivateOneChild(2);
            issueUIManager.Generate(issueRequest);
        });

        CommitsButton.onClick.AddListener(() =>
        {
            singleActiveChild.ActivateOneChild(1);
            GenerateCommits(owner, repo);
        });
        GameSceneMetadata.OnBranchChanged += GameSceneMetadata_OnBranchChanged; 
    }
    private void OnEnable()
    {
        PlayerManager.LocalPlayer.StartInteractingWithUI();
    }
    private void OnDisable()
    {
        PlayerManager.LocalPlayer.StopInteractingWithUI();
    }
    private void GameSceneMetadata_OnBranchChanged()
    {
        currentBranch = GameSceneMetadata.CurrentBranch;
        GitOperations.SwitchBranch(GameSceneMetadata.GithubRepoPath, GameSceneMetadata.CurrentBranch);
        GenerateRepoFiles(owner, repo);
        GenerateCommits(owner, repo);
        issueUIManager.Generate(new RepositoryIssueRequest() { Filter = IssueFilter.All });
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
    public void Show()
    {
        gameObject.SetActive(true);
    }   
    private async void GenerateRepoFiles(string owner, string repoName)
    {
        var contents = await GitHubClientProvider.client.Repository.Content.GetAllContentsByRef(owner, repoName,".",GameSceneMetadata.CurrentBranch);
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