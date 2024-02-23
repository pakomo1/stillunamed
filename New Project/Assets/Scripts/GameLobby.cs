using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Unity.Netcode;
using Unity.Services.Relay.Models;
using System;
using UnityEditor;
using System.IO;
using LibGit2Sharp;

public class GameLobby : MonoBehaviour
{
    private string relayJoinCode = "relayCode";

    public Lobby joinedLobby;
    private float heartBeatTimer;
    [SerializeField] private lobbyTemplate lobbyTemplate;
    [SerializeField] private GameObject nolobbiesText;
    [SerializeField] private GameRelay gameRelay;

    public event EventHandler OnCreateLobbyStarted;
    public event EventHandler OnCreateLobbyFailed;

    public event EventHandler OnLobbyJoinStarted;
    public event EventHandler OnLobbyJoinFailed;

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
        HandleHeartBeatTimer();
    }
    private async void HandleHeartBeatTimer()
    {
        if (joinedLobby != null)
        {
            if (isLobbyHost())
            {
                heartBeatTimer -= Time.deltaTime;
                if (heartBeatTimer < 0f)
                {
                    float heartBeatTimerMax = 15;
                    heartBeatTimer = heartBeatTimerMax;

                    await LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
                }
            }
        }
    }
    public bool isLobbyHost()
    {
        return joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }
    private async void InitializeUnityAuthentication()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            InitializationOptions options = new InitializationOptions();

            options.SetProfile(UnityEngine.Random.Range(0, 1000).ToString());

            await UnityServices.InitializeAsync(options);
            AuthenticationService.Instance.SignedIn += () =>
            {
                print("Singed in" + AuthenticationService.Instance.PlayerId);
            };

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

    }
    public async void CreateLobby(string lobbyname, string githubRepository, bool isPrivate, int maxPlayers, bool shouldFork = true)
    {
        OnCreateLobbyStarted?.Invoke(this, EventArgs.Empty);
        try
        {
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyname, maxPlayers, new CreateLobbyOptions()
            {
                IsPrivate = isPrivate,
            });
            joinedLobby = lobby;
            
            Allocation allocation = await gameRelay.CreateRelay(lobby.MaxPlayers);
            string joinCode = await gameRelay.GetRelayJoinCode(allocation);

           await LobbyService.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
            {
                Data =  new Dictionary<string, DataObject>
                {
                   { relayJoinCode, new DataObject(DataObject.VisibilityOptions.Member, joinCode) }
                }
            });

            var (owner, repoName) = GitHelperMethods.GetOwnerAndRepo(githubRepository);
            
            string currentRepository = githubRepository;
            // check if the repository should be forked
            if (shouldFork)
            {
                //forke that shit
                var forkedRepo = await Forks.ForkRepository(owner, repoName);
                //set the current repository string to the forked repository link
                currentRepository = forkedRepo.HtmlUrl;
            }


            string cloneDirectory = EditorUtility.OpenFolderPanel("Overwrite with folders", "", "All folders");
            //check if the repsitory exits
            if (Repository.IsValid(cloneDirectory)) 
            {
                print("This repo exists");
            }
            else
            {
                print($"Cloning inside: {cloneDirectory}");
                // The repository has not been cloned yet.
                GitOperations.CloneRepository(currentRepository, cloneDirectory);
            }
            GameSceneMetadata.githubRepoLink = currentRepository;
            NetworkManager.Singleton.StartHost();

            //here we should make a couple of scenece with different sizes
            Loader.LoadNetwrok(Loader.Scene.GameScene);

            print($"Created lobby with " + joinedLobby.Name + " " + lobby.IsPrivate);
        }
        catch (LobbyServiceException ex)
        {
            print(ex.Message);
            OnCreateLobbyFailed?.Invoke(this, EventArgs.Empty);
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
            if (queryResponse.Results.Count > 0)
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

        } catch (LobbyServiceException ex)
        {
            print(ex.Message);
        }
    }
    public async void JoinLobbyByID(string id)
    {
        OnLobbyJoinStarted?.Invoke(this, EventArgs.Empty);
        try
        {
            joinedLobby = await Lobbies.Instance.JoinLobbyByIdAsync(id);
            string relayCode = joinedLobby.Data[relayJoinCode].Value;

            JoinAllocation joinAllocation = await gameRelay.JoinRelay(relayCode);
            NetworkManager.Singleton.StartClient();

        } catch (LobbyServiceException ex)
        {
            print(ex.Message);
            OnLobbyJoinFailed?.Invoke(this, EventArgs.Empty);

        }
    }
    public void PrintPlayersInLobby(Lobby lobby)
    {
        foreach (Player player in lobby.Players)
        {
            print(player.Id);
        }
    }
    public Lobby GetLobby()
    {
        return joinedLobby;
    }
    public async void ShutDownLobby(string lobbyId)
    {
        try
        {
             await LobbyService.Instance.DeleteLobbyAsync(lobbyId);
        }catch (LobbyServiceException ex)
        {
            print(ex.Message);
        }
    }
}
