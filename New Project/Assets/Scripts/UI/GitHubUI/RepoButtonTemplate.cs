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
    [SerializeField] private RepoContentNavigation repositoryContentNavigationManager;
    [SerializeField] private RepoButtonTemplateManager repoButtonTemplateManager;
    [SerializeField] private RepositoryOptionsUiManager repositoryOptionsUiManager;

    private void Start()
    {
    }
    public async void CreateButton(Repository currentRepo)
    {
        //Repository repo = await GitHubClientProvider.client.Repository.Get(repoOwner, repoName);

        var button = Instantiate(buttonTemplate, transform);
        button.SetActive(true);

        TextMeshProUGUI repoNameTextMesh = button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        repoNameTextMesh.overflowMode = TextOverflowModes.Ellipsis;
        repoNameTextMesh.text = currentRepo.Name;

        TextMeshProUGUI descriptionTextMesh = button.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        descriptionTextMesh.overflowMode = TextOverflowModes.Ellipsis;
        descriptionTextMesh.text = currentRepo.Description;

        Sprite image = await GetImage(currentRepo.Owner.AvatarUrl);
        button.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = image;
        if (currentRepo.Visibility == RepositoryVisibility.Public)
        {
            button.transform.GetChild(3).GetComponent<Image>().color = Color.green;
        }
        else if (currentRepo.Visibility == RepositoryVisibility.Private)
        {
            button.transform.GetChild(3).GetComponent<Image>().color = Color.red;
        }
        button.GetComponent<Button>().onClick.AddListener(() => repositoryContentNavigationManager.ShowRepositoryContent(currentRepo.Owner.Login, currentRepo.Name, "/"));
        button.GetComponent<RepositoryData>().repository = currentRepo;
    }


    public void OnRightClickHandler(Repository repository, GameObject repoButton)
    {
        // Convert the button's world position to screen position
        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, repoButton.transform.position);

        // Convert the screen position to the UI's local position
        Vector2 localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(repositoryOptionsUiManager.transform.parent.GetComponent<RectTransform>(), screenPosition, null, out localPosition);

        // Adjust the y position to move the UI below the button
        localPosition.y -= repositoryOptionsUiManager.GetComponent<RectTransform>().sizeDelta.y;

        // Set the UI's local position
        repositoryOptionsUiManager.transform.localPosition = localPosition;

        repositoryOptionsUiManager.Show(repository);
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