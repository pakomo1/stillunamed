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
    [SerializeField] private GameObject uiToSetActive;
    [SerializeField] private GameObject sideBarPanel;

    public async void ShowRepositoryContent(string repoOwner, string repoName, string path)
    {
        string url = $"https://api.github.com/repos/{repoOwner}/{repoName}/contents/{path}";
        var accessToken = validAccessToken.GetAccessToken();
        using var www = UnityWebRequest.Get(url);
        www.SetRequestHeader("Authorization", "Bearer " + accessToken);
        var operation = www.SendWebRequest();
        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (www.result == UnityWebRequest.Result.Success)
        {
            print("Successfully fetched repository information" + www.responseCode);
            if (www.responseCode == 200)
            {
                print(www.downloadHandler.text);
                ActivateObjectInContent.OnClickSwitchToThisUI(contentHolder, uiToSetActive);
                UpdateSideBarPanel();
            }
            else
            {
                print("Could not complete task");
            }
        }
        else
        {
            Debug.Log("Error" + www.error);
        }
    }

    private void UpdateSideBarPanel()
    {
        var description = sideBarPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;
        var stars = sideBarPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text;
        var watching = sideBarPanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text;
        var forks = sideBarPanel.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text;
    }
}
