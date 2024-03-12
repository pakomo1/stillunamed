using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Octokit;
using Unity.Services.Lobbies.Models;
using System;
using System.Threading.Tasks;

public class CreateLobbyUI : MonoBehaviour
{
    [SerializeField] private Button createButton;
    [SerializeField] private TMP_InputField lobbyName;
    [SerializeField] private TMP_InputField repoLink;
    [SerializeField] private TMP_InputField maxPlayerCount;
    [SerializeField] private Toggle isPrivate;

    [SerializeField] private GameObject alertForkingUI;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button cancelButton;

    [SerializeField] private GameLobby gameLobby;
    [SerializeField] private GameRelay gameRelay;


    private bool shouldContinue;
    private void Start()
    {
        continueButton.onClick.AddListener(() =>
        {
            shouldContinue = true;
            alertForkingUI.SetActive(false);
        });

        cancelButton.onClick.AddListener(() =>
        {
            shouldContinue = false;
            alertForkingUI.SetActive(false);
        });


        createButton.onClick.AddListener(async () =>
        {
            bool isReal = await CheckIfRepoLinkIsReal(repoLink.text);
            if (isReal)
            {
                bool createFork = true;
                bool isOwner = await CheckIfUserIsOwnerOfRepo(repoLink.text);
                if (isOwner)
                {
                   createFork = false;
                }
                else
                {
                    alertForkingUI.SetActive(true);
                    await WaitForUserResponse();
                    if (!shouldContinue)
                    {
                        return;
                    }
                }
                gameLobby.CreateLobby(lobbyName.text, repoLink.text, isPrivate.isOn, int.Parse(maxPlayerCount.text), createFork);
                lobbyName.text = "";
                repoLink.text = "";
                maxPlayerCount.text = "0";

                Lobby joinedLobby = gameLobby.GetLobby();

                Hide();
            }
            else
            {
                repoLink.text = "";
            }
            
        });

        if (Input.GetKey(KeyCode.Escape))
        {
            Hide();
        }
    }

    private async Task<bool> CheckIfUserIsOwnerOfRepo(string url)
    {
        try
        {
            var (owner, repoName) = GitHelperMethods.GetOwnerAndRepo(url);
            var repository = await GitHubClientProvider.client.Repository.Get(owner, repoName);
            var currentUser = await GitHubClientProvider.client.User.Current();
            return repository.Owner.Login == currentUser.Login;
        }
        catch (Exception error)
        {
            // Handle error
            return false;
        }
    }
    public async Task<bool> CheckIfRepoLinkIsReal(string url)
    {
        try
        {
            var (owner, repoName)= GitHelperMethods.GetOwnerAndRepo(url);
            var repository = await GitHubClientProvider.client.Repository.Get(owner, repoName);
            return true;

        }catch (Exception ex)
        {
            //Display exception
            print("Repository NotFound");
            return false;
        }
    }
  
    public void Show(Repository repository =null)
    {
        if(repository != null)
        {
           repoLink.text = repository.HtmlUrl;
        }
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private async Task WaitForUserResponse()
    {
        while (alertForkingUI.activeSelf)
        {
            await Task.Delay(100);
        }
    }

}
