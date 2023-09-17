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
    [SerializeField] private lobbyTemplate lobbyTemplate;
    [SerializeField] private GameObject nolobbiesText;
    public static GameLobby Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        InitializeUnityAuthentication();
    }

    private async void InitializeUnityAuthentication()
    {
        if(UnityServices.State != ServicesInitializationState.Initialized)
        {
            InitializationOptions options = new InitializationOptions();

            //the profile name should be the person's github username
            options.SetProfile(Random.Range(0,1000).ToString());

            await UnityServices.InitializeAsync(options);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        
    }
    public async void CreateLobby(string lobbyname, string githubRepository, bool isPrivate, int maxPlayers)
    {
        try
        {
            joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyname, maxPlayers, new CreateLobbyOptions()
            {
                IsPrivate = isPrivate
            });
            LobbyUi.StartHost();
            Loader.LoadNetwrok(Loader.Scene.GameScene);
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
            if(queryResponse.Results.Count > 0)
            {
                nolobbiesText.SetActive(false);
                foreach (Lobby lobby in queryResponse.Results)
                {
                    lobbyTemplate.GenerateLobbies(lobby.Name, lobby.MaxPlayers);
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

}
