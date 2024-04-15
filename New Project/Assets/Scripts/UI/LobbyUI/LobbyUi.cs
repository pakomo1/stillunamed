using Octokit;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class LobbyUi : MonoBehaviour
{
    [SerializeField] private GameLobby gameLobby;
    [SerializeField] private CreateLobbyUI createLobbyUI;
    [SerializeField] private DataBaseManager dbManager;

    [SerializeField] private TextMeshProUGUI noLobbiesFound;
    [SerializeField] private TextMeshProUGUI explorerLable;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject connectingUI;
    [SerializeField] private GameObject networkManager;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject shouldLoginPopUp;
    [SerializeField] private Button createLobbyBtn;
    [SerializeField] private Button refreshButton;
    [SerializeField] private Button recentLobbiesButton;



    public event EventHandler OnTryToJoinGame; 
    public event EventHandler OnFaildToJoinGame;


    public static LobbyUi Instance { get; private set; }
    private void Start()
    {

        gameLobby.GetAllPublicLobbies();
        
        createLobbyBtn.onClick.AddListener(() => 
        {
           createLobbyUI.Show();
        });
        refreshButton.onClick.AddListener(() =>
        {
            Clear();
            gameLobby.GetAllPublicLobbies();
        });

        recentLobbiesButton.onClick.AddListener(onClickRecenLobbyButton);
    }
    private void OnEnable()
    {
        if (!IsLoggedIn())
        {
            gameObject.SetActive(false);
            shouldLoginPopUp.SetActive(true);
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {

    }
    private void Clear()
    {
        for (int i = 0; i < content.transform.childCount; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
    }

    private async void onClickRecenLobbyButton()
    {
        Clear();
        try
        {
            explorerLable.text = "Recent Lobbies";
            string username = GitHubClientProvider.client.User.Current().Result.Login;
            var recentLobbies = await dbManager.GetRecentLobbies(username);

            List<   Lobby> recentLobbiesList = new List<Lobby>();
            for (int i = 0; i < recentLobbies.Count; i++)
            {
                try
                {
                    var lobby = await gameLobby.GetLobbyById(recentLobbies[i]);
                    recentLobbiesList.Add(lobby);
                }
                catch (LobbyServiceException)
                {
                    recentLobbies.RemoveAt(i);
                    i--;
                }
            }

            gameLobby.ListLobbies(recentLobbiesList);

            if (recentLobbies.Count == 0)
            {
                noLobbiesFound.gameObject.SetActive(true);
                noLobbiesFound.text = "There are no active lobbies that you have joined recently";
                return;
            }
        }catch(Exception ex)
        {
            Debug.LogError(ex);
        }   
        
    }

 

    private void NetwrokManager_OnClientDisconnectCallback(ulong clientId)
    {
        OnFaildToJoinGame?.Invoke(this, EventArgs.Empty);
    }
    public void Show()
    {
       networkManager.SetActive(true);
       gameObject.SetActive(true);
    }
    public void Hide()
    {
        menu.gameObject.SetActive(false);
    }
    //checks if there is an access token 
    public bool IsLoggedIn()
    {
        return GitHubClientProvider.client.Connection.Credentials.AuthenticationType == AuthenticationType.Oauth;
    }
    //hide the login popup
    public void HideLoginPopUp()
    {
        shouldLoginPopUp.SetActive(false);
    }
}
