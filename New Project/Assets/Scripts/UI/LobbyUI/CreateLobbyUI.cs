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
    
    [SerializeField] private GameLobby gameLobby;
    [SerializeField] private GameRelay gameRelay;
    private void Start()
    {
        createButton.onClick.AddListener(async () =>
        {
            bool isReal = await CheckIfRepoLinkIsReal(repoLink.text);
            if (isReal)
            {
                bool isOwner = await CheckIfUserIsOwnerOfRepo(repoLink.text);
                if (isOwner)
                {
                    // Show options to use the repository directly or create a fork
                }
                else
                {
                    GameSceneMetadata.githubRepoLink = repoLink.text;
                    gameLobby.CreateLobby(lobbyName.text, repoLink.text, isPrivate.isOn, int.Parse(maxPlayerCount.text), true);
                    lobbyName.text = "";
                    repoLink.text = "";
                    maxPlayerCount.text = "0";

                    Lobby joinedLobby = gameLobby.GetLobby();

                    Hide();
                }

                
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
            print("Repository Exists");
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
           repoLink.text = repository.GitUrl;
        }
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
