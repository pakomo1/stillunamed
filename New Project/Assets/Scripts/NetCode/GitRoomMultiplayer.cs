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
