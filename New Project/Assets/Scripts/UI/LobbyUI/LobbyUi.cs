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

public class LobbyUi : MonoBehaviour
{
    [SerializeField] private GameLobby gameLobby;
    [SerializeField] private CreateLobbyUI createLobbyUI;
    [SerializeField] private DataBaseManager dbManager;

    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject connectingUI;
    [SerializeField] private GameObject networkManager;
    [SerializeField] private GameObject content;
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
            string username = GitHubClientProvider.client.User.Current().Result.Login;
            var recentLobbies = await dbManager.GetRecentLobbies(username);
            if(recentLobbies.Count == 0)
            {
                print("You haven't joined any lobbies recently");
                return;
            }
            List<Lobby> recentLobbiesList = new List<Lobby>();
            foreach (var lobbyID in recentLobbies)
            {
                var lobby = await gameLobby.GetLobbyById(lobbyID);
                if (lobby == null)
                {
                    await dbManager.RemoveRecentLobby(username, lobbyID);
                    continue;
                }
                recentLobbiesList.Add(lobby);
            }
            gameLobby.ListLobbies(recentLobbiesList);
        }catch(Exception ex)
        {
            print(ex);
        }
       
    }

    public static void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }
    public static void StartClient()
    {
        Instance.OnTryToJoinGame?.Invoke(Instance, EventArgs.Empty);
        NetworkManager.Singleton.OnClientDisconnectCallback += Instance.NetwrokManager_OnClientDisconnectCallback;
        
        NetworkManager.Singleton.StartClient();
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
}
