using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Unity.Netcode;
using Unity.Services.Relay.Models;
using System;
using System.Threading.Tasks;
using TMPro;
using System.Linq;
using Unity.Services.Vivox;


public class GameLobby : MonoBehaviour
{
    private string relayJoinCode = "relayCode";
    private string joinLobbyCode = "joinLobbyCode";

    public Lobby joinedLobby;
    public Lobby hostLobby;
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
    public event EventHandler<LobbyJoinEventArgs> OnPlayerTriesToJoin;

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
    //lists the lobbies periodically
    public void ListLobbiesPeriodically()
    {
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
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            await VivoxService.Instance.InitializeAsync();

            AuthenticationService.Instance.SignedIn += () =>
            {
                print("Singed in" + AuthenticationService.Instance.PlayerId);
            };

        }

    }
    public async void CreateLobby(string lobbyname, string githubRepository, bool isPrivate, int maxPlayers, bool shouldFork = true)
    {
        OnCreateLobbyStarted?.Invoke(this, EventArgs.Empty);
        try
        {

            var (owner, repoName) = GitHelperMethods.GetOwnerAndRepo(githubRepository);
            var user = await GitHubClientProvider.client.User.Current();
            bool isOwner = await GitOperations.IsUserRepoOwnerAsync(user.Login, githubRepository);
            string currentRepository = githubRepository;
            // check if the repository should be forked
            if (shouldFork)
            {
                OnForkStarted?.Invoke(this, EventArgs.Empty);
                try
                {
                    var forkedRepo = await Forks.ForkRepository(owner, repoName);
                    //set the current repository string to the forked repository link
                    currentRepository = forkedRepo.HtmlUrl;

                    OnForkSuccess?.Invoke(this, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    OnForkFailed?.Invoke(this, new GitOperationsEventArgs(ex.Message));
                    return;
                }

            }
            string cloneDirectory = await SelectFolder();
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
                }
                catch (Exception er)
                {
                    OnCloneFailed.Invoke(this, new GitOperationsEventArgs(er.Message));
                    return;
                }
            }
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyname, maxPlayers, new CreateLobbyOptions()
            {
                IsPrivate = isPrivate,
                Player = GetPlayer(user.Login, isOwner)
            });
            joinedLobby = lobby;
            hostLobby = lobby;

            VivoxManager.Instance.LoginToVivoxAsync(user.Login);
            

            //lobby events. The lobby service is listening for these events
            var callbacks = new LobbyEventCallbacks();
            callbacks.PlayerJoined += (List<LobbyPlayerJoined> obj) => { Callbacks_PlayerJoined(obj, githubRepository); };
            await LobbyService.Instance.SubscribeToLobbyEventsAsync(lobby.Id, callbacks);

            Allocation allocation = await gameRelay.CreateRelay(lobby.MaxPlayers - 1);
            string joinCode = await gameRelay.GetRelayJoinCode(allocation);


            string channelName = "channel-" + joinedLobby.Id;
            VivoxManager.Instance.JoinhannelAsync(channelName);

            await LobbyService.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    { relayJoinCode, new DataObject(DataObject.VisibilityOptions.Member, joinCode) },
                    { "repoLink", new DataObject(DataObject.VisibilityOptions.Member, currentRepository) }, // Add this line
                    { joinLobbyCode, new DataObject(DataObject.VisibilityOptions.Member, lobby.LobbyCode) }
                }
            });
    
            GameSceneMetadata.GithubRepoLink = currentRepository;
            GameSceneMetadata.GithubRepoPath = repoPath;
            GameSceneMetadata.CurrentBranch = GitOperations.GetCurrentBranch(repoPath);

            NetworkManager.Singleton.StartHost();

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

    private async void Callbacks_PlayerJoined(List<LobbyPlayerJoined> obj, string repositoryLink)
    {
        //prints the player id
        print(obj[0].Player.Id);
        Lobby lobby = await LobbyService.Instance.GetLobbyAsync(Instance.joinedLobby.Id);


        // Retrieve the username from the player's data
        string username = obj[0].Player.Data["username"].Value;

        var user = GitHubClientProvider.client.User.Current().Result;
        //so this value will be null if there is no owner set
        var owner = joinedLobby.Players.FirstOrDefault(p => bool.Parse(p.Data["isOwner"].Value));
        if (owner != null)
        {
            bool isCollaborator = await GitOperations.IsUserCollaboratorAsync(username, repositoryLink);
            if (!isCollaborator)
            {
                print("Sending invite");
                await GitOperations.InviteUserToRepoAsync(username, repositoryLink);

            }
            else
            {
                print("The player is already a colaborator");
            }
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

    public void ListLobbies(List<Lobby> lobbies)
    {
        foreach (Lobby lobby in lobbies)
        {
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
            JoinLobbyByIdOptions options = new JoinLobbyByIdOptions();
            options.Player = GetPlayer(user.Login, false);

            joinedLobby = await Lobbies.Instance.JoinLobbyByIdAsync(id, options);
            string relayCode = joinedLobby.Data[relayJoinCode].Value;
            string repoLink = joinedLobby.Data["repoLink"].Value;
            
            //Login to the vivox
            VivoxManager.Instance.LoginToVivoxAsync(user.Login);

            var isOwner = await GitOperations.IsUserRepoOwnerAsync(user.Login, repoLink);
            options.Player = GetPlayer(user.Login, isOwner);

            OnPlayerTriesToJoin?.Invoke(this, agrs);

            await dbManager.UpdateRecentLobbies(user.Login, joinedLobby.Id);
            await gameRelay.JoinRelay(relayCode);

            //join the channel
            string channelName = "channel-" + joinedLobby.Id;
            VivoxManager.Instance.JoinhannelAsync(channelName);

            string cloneDirectory = await SelectFolder();
            //check the if the user has selected a cloneDirecotry
            if (string.IsNullOrEmpty(cloneDirectory))
            {
                //leave the lobby
                LeaveLobby();
                throw new Exception("No directory selected");
            }
            string repoName = GitHelperMethods.GetOwnerAndRepo(repoLink).repoName;
            string repoPath = @$"{cloneDirectory}\{repoName}";

            if (LibGit2Sharp.Repository.IsValid(repoPath))
            {
                using (var repo = new LibGit2Sharp.Repository(repoPath))
                {
                    var remoteUrl = repo.Network.Remotes["origin"].Url;
                    if (remoteUrl != repoLink)
                    {
                        throw new Exception("You should select a different path because there is already a repository here");
                    }
                }
            }
            else
            {
                await GitOperations.CloneRepositoryAsync(repoLink, repoPath);
            }

            GameSceneMetadata.GithubRepoLink = repoLink;
            GameSceneMetadata.GithubRepoPath = repoPath;
            GameSceneMetadata.CurrentBranch = GitOperations.GetCurrentBranch(repoPath);

            OnLobbyJoinStarted?.Invoke(this, agrs);
            GitRoomMultiplayer.Instance.StartClient();

        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            LeaveLobby();
            OnLobbyJoinFailed?.Invoke(this, EventArgs.Empty);
        }
    }
    public async void JoinPrivateLobby(string code)
    {
        var user = await GitHubClientProvider.client.User.Current();

        var args = new LobbyJoinEventArgs(user.Login);
        try
        {
            JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions();
            options.Player = GetPlayer(user.Login, false);

            joinedLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(code, options);
            string relayCode = joinedLobby.Data[relayJoinCode].Value;
            string repoLink = joinedLobby.Data["repoLink"].Value;
            string channelName = "channel-" + joinedLobby.Id;

            bool isCollaborator = false;
            // Wait for the user to accept the invitation.
            while (!isCollaborator)
            {
                try
                {
                    isCollaborator = await GitOperations.IsUserCollaboratorAsync(user.Login, repoLink);
                    if (isCollaborator)
                    {
                        break; // Exit the loop if the user is a collaborator.
                    }
                }
                catch (Octokit.NotFoundException)
                {
                    // If the user does not have access to the repository, wait for 10 seconds and then try again.
                    await Task.Delay(TimeSpan.FromSeconds(10));
                }

                OnPlayerTriesToJoin?.Invoke(this, args);
            }
            var isOwner = await GitOperations.IsUserRepoOwnerAsync(user.Login, repoLink);
            options.Player = GetPlayer(user.Login, isOwner);

            await dbManager.UpdateRecentLobbies(user.Login, joinedLobby.Id);
            await gameRelay.JoinRelay(relayCode);

            string cloneDirectory = await SelectFolder();
            //check the if the user has selected a cloneDirecotry
            if (string.IsNullOrEmpty(cloneDirectory))
            {
                //leave the lobby
                LeaveLobby();
                throw new Exception("No directory selected");
            }
            string repoName = GitHelperMethods.GetOwnerAndRepo(repoLink).repoName;
            string repoPath = @$"{cloneDirectory}\{repoName}";

            if (LibGit2Sharp.Repository.IsValid(repoPath))
            {
                using (var repo = new LibGit2Sharp.Repository(repoPath))
                {
                    var remoteUrl = repo.Network.Remotes["origin"].Url;
                    if (remoteUrl != repoLink)
                    {
                        throw new Exception("You should select a different path because there is already a repository here");
                    }
                }
            }
            else
            {
                await GitOperations.CloneRepositoryAsync(repoLink, repoPath);
            }

            GameSceneMetadata.GithubRepoLink = repoLink;
            GameSceneMetadata.GithubRepoPath = repoPath;
            GameSceneMetadata.CurrentBranch = GitOperations.GetCurrentBranch(repoPath);

            OnLobbyJoinStarted?.Invoke(this, args);
            GitRoomMultiplayer.Instance.StartClient();

        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            LeaveLobby();
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
            throw ex;
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
            joinedLobby = null;
        }
        catch (LobbyServiceException ex)
        {
            print(ex.Message);
        }
    }
    //leaves the lobby
    public async void LeaveLobby()
    {
        if (joinedLobby != null)
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
                joinedLobby = null;
            }
            catch (LobbyServiceException ex)
            {
                print(ex.Message);
            }
        }
    }
    private Player GetPlayer(string username, bool isOwner)
    {

        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
            {
               {
                    "username", new PlayerDataObject(
                    visibility: PlayerDataObject.VisibilityOptions.Public,
                    value: username)
               },
               {
                "isOwner", new PlayerDataObject(
                visibility: PlayerDataObject.VisibilityOptions.Public,
                value: isOwner.ToString())
               }
            }
        };
    }
    //migrates the lobby host
    private async void MigrateHost()
    {
        try
        {
            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions
            {
                HostId = joinedLobby.Players[1].Id
            });
            joinedLobby = hostLobby;
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e.Message);
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
