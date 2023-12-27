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
                gameLobby.CreateLobby(lobbyName.text, repoLink.text, isPrivate.isOn, int.Parse(maxPlayerCount.text));
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
    private async Task<bool> CheckIfRepoLinkIsReal(string url)
    {
        try
        {
            var (owner, repoName)= GetOwnerAndRepo(url);
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
    private static(string owner, string repoName) GetOwnerAndRepo(string repoUrl)
    {
        Uri repoUri = new Uri(repoUrl);
        
        if(repoUri.Segments.Length >= 3)
        {
            string owner = repoUri.Segments[1].TrimEnd('/');
            string repoName = repoUri.Segments[2].TrimEnd('/');
            repoName = repoName.Substring(0,repoName.Length - 4);

            Debug.Log(repoName);
            return(owner, repoName);
        }
        throw new ArgumentException("Invalid GitHub repository URL");
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
