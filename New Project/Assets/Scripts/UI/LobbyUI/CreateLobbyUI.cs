using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Octokit;
using Unity.Services.Lobbies.Models;

public class CreateLobbyUI : MonoBehaviour
{
    [SerializeField] private Button createButton;
    [SerializeField] private TMP_InputField lobbyName;
    [SerializeField] private TMP_InputField repoLink;
    [SerializeField] private MaxPlayersManager maxPlayersManager;
    

    [SerializeField] private Toggle isPrivate;
    
    [SerializeField] private GameLobby gameLobby;
    [SerializeField] private GameRelay gameRelay;
    private void Start()
    {
        createButton.onClick.AddListener(() =>
        {

           
            gameLobby.CreateLobby(lobbyName.text, repoLink.text, isPrivate.isOn, maxPlayersManager.selectedPlayerCount);
            lobbyName.text = "";
            repoLink.text = "";
            

            Lobby joinedLobby = gameLobby.GetLobby();

            Hide();
        });

        if (Input.GetKey(KeyCode.Escape))
        {
            Hide();
        }
    }
    private void CheckIfRepoLinkIsReal(string url)
    {
        // check if the link is real by making a request to the github api 
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
