using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft;
using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class RepostoryViewerManager : MonoBehaviour
{
    [SerializeField] private Image repoOwenerProfilePicture;
    [SerializeField] private TextMeshProUGUI repoOwnerName;
    [SerializeField] private TextMeshProUGUI repositoryNameLbl;
    [SerializeField] private RepoFilesTemplate repoFilesTemplate;
    [SerializeField] private FilesContentNavigation filesContentNavigation;
    public string currentBranch = "main";

    private void OnEnable()
    {
        var uri = new Uri(GameSceneMetadata.GithubRepoLink);
        var segments = uri.AbsolutePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        string owner = segments[0];
        string repo = segments[1];
        SetProfilePicture(owner);
        repoOwnerName.text = owner;
        GenerateRepoFiles(owner,repo);
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

}