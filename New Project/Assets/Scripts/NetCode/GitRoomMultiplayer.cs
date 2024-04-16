using System;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;

public class GitRoomMultiplayer : NetworkBehaviour
{
    public event EventHandler OnPlayerTriesToJoin;
    public event EventHandler OnPlayerFaildToJoin;
    public static GitRoomMultiplayer Instance { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        GameLobby.Instance.OnPlayerTriesToJoin += Instance_OnPlayerTriesToJoin;
        NetworkManager.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            // If the connection fails, remove the player from the lobby
            string playerId = AuthenticationService.Instance.PlayerId;
            LobbyService.Instance.RemovePlayerAsync(GameLobby.Instance.joinedLobby.Id, playerId);
        }
    }

    private void Instance_OnPlayerTriesToJoin(object sender, LobbyJoinEventArgs e)
    {
        print(e.Username);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //starts the client
    public void StartClient()
    {
        OnPlayerTriesToJoin?.Invoke(this, new LobbyJoinEventArgs("Player"));
        NetworkManager.Singleton.StartClient();
    }


    //starts the host
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }
}
