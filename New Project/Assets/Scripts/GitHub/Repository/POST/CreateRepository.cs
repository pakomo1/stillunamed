using Newtonsoft.Json;
using Octokit;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CreateRepository : MonoBehaviour
{
    [SerializeField] private ToggleGroup visibilityOptions;
    [SerializeField] private Toggle readmeFileInitBtn;
    [SerializeField] private TextMeshProUGUI nameOfRepo;
    [SerializeField] private ValidAccessToken validAccessToken;
    [SerializeField] private FilteringController filteringController;
    [SerializeField] private GameObject createdPopUp;
    private string url = "https://api.github.com/user/repos";
    private bool isPrivate;
    private bool readmeInitialized;

    public async void ActivateRequest()
    {
        Toggle chosenVisibilityOption = visibilityOptions.ActiveToggles().FirstOrDefault();
        if (chosenVisibilityOption.name == "PrivateRadioBtn")
        {
            isPrivate = true;
        }
        else
        {
            isPrivate = false;
        }

        if (readmeFileInitBtn.isOn)
        {
            readmeInitialized = true;
        }
        else
        {
            readmeInitialized = false;
        }

        var newRepository = new NewRepository(nameOfRepo.text.Trim())
        {
            Private = isPrivate,
            AutoInit = readmeInitialized
        };

        var client = GitHubClientProvider.client;

        try
        {
            var repository = await client.Repository.Create(newRepository);
            Debug.Log("Repository successfully created");
            filteringController.Refresh();
            createdPopUp.SetActive(true);
            ClearFields();
        }
        catch (Exception ex)
        {
            Debug.Log($"Could not create repository: {ex.Message}");
        }
    }
    //clears all fields
    public void ClearFields()
    {
        nameOfRepo.text = "";
        readmeFileInitBtn.isOn = false;
        visibilityOptions.SetAllTogglesOff();
    }
    public void ClosePopUp()
    {
        createdPopUp.SetActive(false);
    }
}
