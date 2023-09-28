using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class GameLobby : MonoBehaviour
{
    private Lobby joinedLobby;
    private float heartBeatTimer;
    [SerializeField] private lobbyTemplate lobbyTemplate;
    [SerializeField] private GameObject nolobbiesText;
    public static GameLobby Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        InitializeUnityAuthentication();
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
       
    }

    private void Update()
    {
        //HandleHeartBeatTimer();
    }
    private async void HandleHeartBeatTimer()
    {
       if(joinedLobby != null)
        {
            heartBeatTimer -= Time.deltaTime;
            if(heartBeatTimer < 0f)
            {
                float heartBeatTimerMax = 15;
                heartBeatTimer = heartBeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }
        }
    }

    private async void InitializeUnityAuthentication()
    {
        if(UnityServices.State != ServicesInitializationState.Initialized)
        {
            InitializationOptions options = new InitializationOptions();
            //the profile name should be the person's github username
            options.SetProfile(Random.Range(0,1000).ToString());
            
            await UnityServices.InitializeAsync(options);
            AuthenticationService.Instance.SignedIn += () =>
            {
                print("Singed in" + AuthenticationService.Instance.PlayerId);
            };

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        
    }
    public async void CreateLobby(string lobbyname, string githubRepository, bool isPrivate, int maxPlayers)
    {
        try
        {
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyname, maxPlayers, new CreateLobbyOptions()
            {
                IsPrivate = isPrivate,
            });

            
            joinedLobby = lobby;
            print("Created lobby with " + joinedLobby.Name + " " + lobby.IsPrivate);
        }
        catch(LobbyServiceException ex)
        {
            print(ex.Message);
        }
        
    }
    public async void QuickJoin()
    {
        try
        {
            joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            LobbyUi.StartClient();
        }
        catch (LobbyServiceException ex)
        {
            print(ex.Message);
        }
    }

    public async void ListLobbies()
    {
        try
        {
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
            print("Number of lobbies found" + queryResponse.Results.Count);
            if(queryResponse.Results.Count > 0)
            {
                nolobbiesText.SetActive(false);
                foreach (Lobby lobby in queryResponse.Results)
                {
                    print(lobby.Name);

                    lobbyTemplate.GenerateLobbie(lobby);
                }
            }
            else
            {
                nolobbiesText.SetActive(true);
            }
            
        }catch (LobbyServiceException ex)
        {
            print(ex.Message);
        }
    }
    public async void JoinLobbyByID(string id)
    {
        try
        {
            await Lobbies.Instance.JoinLobbyByIdAsync(id);
        }catch (LobbyServiceException ex) 
        {
            print(ex.Message);
        }
    }
    public void PrintPlayersInLobby(Lobby lobby)
    {
        foreach (Player player in lobby.Players)
        {
            print(player.Id);
        }
    }

}
