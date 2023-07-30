using Newtonsoft.Json;
using System;
using System.Collections;
using System.Linq;
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
    private string url = "https://api.github.com/user/repos";
    private bool isPrivate;
    private bool readmeInitialized;

    public void ActivateRequest()
    {
        StartCoroutine(SendRequestToCreateRepository());
    }

    private IEnumerator SendRequestToCreateRepository()
    {
        Toggle chosenVisibilityOption = visibilityOptions.ActiveToggles().FirstOrDefault();
        var accessToken = validAccessToken.GetAccessToken();

        if (chosenVisibilityOption.name == "PrivateRadioBtn")
        {
            isPrivate = true;//true
        }
        else
        {
            isPrivate = false;//false
        }

        if (readmeFileInitBtn.isOn)
        {
            readmeInitialized = true;//true
        }
        else
        {
            readmeInitialized = false;//false
        }

        RequestParameters requestparam = new RequestParameters
        {
            name = nameOfRepo.text,
            Private = isPrivate,
            auto_init = readmeInitialized
        };
        string jsonText = JsonConvert.SerializeObject(requestparam, Formatting.Indented);
        using (var www = UnityWebRequest.Post(url, jsonText))
        {
            www.SetRequestHeader("Authorization", "Bearer " + accessToken);
            www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonText));
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                print("Repository response code " + www.responseCode);
                if (www.responseCode == 201)
                {
                    print("Repository succesfully created");
                    filteringController.Refresh();
                }
                else
                {
                    print("Could not create repository");
                }
            }
            else
            {
                Debug.Log("Error" + www.error);
            }
        }
    }

    [Serializable]
    public class RequestParameters
    {
        public string name;
        [JsonProperty("private")]
        public bool Private;
        public bool auto_init;
    }
}
