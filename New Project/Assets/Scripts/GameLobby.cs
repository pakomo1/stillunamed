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
using SFB;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Relay;
using Octokit;

public class GameLobby : MonoBehaviour
{
    private string relayJoinCode = "relayCode";

    public Lobby joinedLobby;
    private float heartBeatTimer;
    [SerializeField] private lobbyTemplate lobbyTemplate;
    [SerializeField] private GameRelay gameRelay;
    [SerializeField] private DataBaseManager dbManager;

    [SerializeField] private TextMeshProUGUI noLobbiesFound;
    [SerializeField] private TextMeshProUGUI explorerLable;

    //lobby events
    public event EventHandler OnCreateLobbyStarted;
    public event EventHandler OnCreateLobbyFailed;

    public event EventHandler<LobbyJoinEventArgs> OnLobbyJoinStarted;
    public event EventHandler OnLobbyJoinFailed;
    
    //git events
    public event EventHandler OnForkStarted;
    public event EventHandler<GitOperationsEventArgs> OnForkFailed;
    public event EventHandler OnForkSuccess;

    public event EventHandler OnCloneStarted;
    public event EventHandler<GitOperationsEventArgs> OnCloneFailed;
    public event EventHandler OnCloneSuccess;

    //gamescene events
    public static event EventHandler<LobbyJoinEventArgs> OnPlayerTriesToJoin;

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
            
            var (owner, repoName) = GitHelperMethods.GetOwnerAndRepo(githubRepository);
            
            string currentRepository = githubRepository;
            // check if the repository should be forked
            if (shouldFork)
            {

                OnForkStarted?.Invoke(this, EventArgs.Empty);
                try
                {
                    //forke that shit
                    var forkedRepo = await Forks.ForkRepository(owner, repoName);
                    //set the current repository string to the forked repository link
                    currentRepository = forkedRepo.HtmlUrl;

                    OnForkSuccess?.Invoke(this, EventArgs.Empty);
                }
                catch(Exception ex)
                {
                    OnForkFailed?.Invoke(this, new GitOperationsEventArgs(ex.Message));
                    return;
                }
              
            }
            string cloneDirectory =await SelectFolder();
            string repoPath = @$"{cloneDirectory}\{repoName}";
            //check if the repsitory exits
            if (LibGit2Sharp.Repository.IsValid(repoPath)) 
            {
                print("This repo exists");
            }
            else
            {
                print($"Cloning inside: {cloneDirectory[0]}");
                OnCloneStarted?.Invoke(this, EventArgs.Empty);
                try
                {
                    // The repository has not been cloned yet.
                    await GitOperations.CloneRepositoryAsync(currentRepository, repoPath);
                }catch(Exception er)
                {
                    OnCloneFailed.Invoke(this, new GitOperationsEventArgs(er.Message));
                    return;
                }
               
            }
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyname, maxPlayers, new CreateLobbyOptions()
            {
                IsPrivate = isPrivate,
            });
            joinedLobby = lobby;

            Allocation allocation = await gameRelay.CreateRelay(lobby.MaxPlayers - 1);
            string joinCode = await gameRelay.GetRelayJoinCode(allocation);

            await LobbyService.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                   { relayJoinCode, new DataObject(DataObject.VisibilityOptions.Member, joinCode) }
                }
            });

            GameSceneMetadata.GithubRepoLink = currentRepository;
            GameSceneMetadata.GithubRepoPath = repoPath;
            GameSceneMetadata.CurrentBranch = GitOperations.GetCurrentBranch(repoPath);
            NetworkManager.Singleton.StartHost();

            OnLobbyJoinStarted += GameLobby_OnLobbyJoinStarted;

            //here you should make a couple of scenece with different room sizes
            Loader.LoadNetwrok(Loader.Scene.GameScene);


            print($"Created lobby with " + joinedLobby.Name + " " + lobby.IsPrivate);
        }
        catch (LobbyServiceException ex)
        {
            print(ex.Message);
            OnCreateLobbyFailed?.Invoke(this, EventArgs.Empty);
        }

    }
    public Task<string> SelectFolder()
    {
        var tcs = new TaskCompletionSource<string>();

        SimpleFileBrowser.FileBrowser.ShowLoadDialog((path) =>
        {
            // This is called when the user selects a directory
            tcs.SetResult(path[0]);
        },
        () =>
        {
            // This is called when the user cancels the dialog
            tcs.SetResult(null);
        },
        SimpleFileBrowser.FileBrowser.PickMode.Folders, false, null, "Select Folder", "Select");

        return tcs.Task;
    }

    private void GameLobby_OnLobbyJoinStarted(object sender, LobbyJoinEventArgs args)
    {
        print($"The player: {args.Username}");
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

    public void ListLobbies(List<Lobby> lobbies)
    {
        foreach (Lobby lobby in lobbies )
        {
            print(lobby.Name);
            lobbyTemplate.GenerateLobbie(lobby);
        }
    }
    public async void GetAllPublicLobbies()
    {
        try
        {
            explorerLable.text = "Lobby Explorer";
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
            print("Number of lobbies found" + queryResponse.Results.Count);
            if (queryResponse.Results.Count > 0)
            {
                ListLobbies(queryResponse.Results);
            }
            else
            {
                noLobbiesFound.text = "There aren't any active lobbies";
                noLobbiesFound.gameObject.SetActive(true);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    public async void JoinLobbyByID(string id)
    {
        var user = await GitHubClientProvider.client.User.Current();
        var agrs = new LobbyJoinEventArgs(user.Login);
        try
        {
            joinedLobby = await Lobbies.Instance.JoinLobbyByIdAsync(id);
            string relayCode = joinedLobby.Data[relayJoinCode].Value;

            OnPlayerTriesToJoin?.Invoke(this, agrs);

            await dbManager.UpdateRecentLobbies(user.Login,joinedLobby.Id);
            await gameRelay.JoinRelay(relayCode);

            OnLobbyJoinStarted?.Invoke(this, agrs);
            NetworkManager.Singleton.StartClient();

        } catch (Exception ex)
        {
            Debug.LogError(ex);
            OnLobbyJoinFailed?.Invoke(this, EventArgs.Empty);
        }
    }
    public async Task<Lobby> GetLobbyById(string id)
    {
        try
        {
            Lobby lobby = await Lobbies.Instance.GetLobbyAsync(id);
            return lobby;
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogError("Failed to retrieve lobby: " + ex.Message);
            return null;
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
public class LobbyJoinEventArgs : EventArgs
{
    public string Username { get; set; }

    public LobbyJoinEventArgs(string username)
    {
        Username = username;
    }
}
