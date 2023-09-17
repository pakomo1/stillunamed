using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUi : MonoBehaviour
{
    [SerializeField] private Button createGame;
    [SerializeField] private Button joinGame;
    [SerializeField] private GameObject connectingUI;
    [SerializeField] private GameLobby gameLobby;

    public event EventHandler OnTryToJoinGame; 
    public event EventHandler OnFaildToJoinGame;

    public static LobbyUi Instance { get; private set; }
    private void Start()
    {
       /* connectingUI.SetActive(true);
        createGame.onClick.AddListener(() =>
        {
            StartHost();
            Loader.LoadNetwrok(Loader.Scene.GameScene);
        });
        joinGame.onClick.AddListener(() =>
        {
            StartClient();
        });*/

        gameLobby.ListLobbies();
    }

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
      
    }

    public static void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }
    public static void StartClient()
    {
        Instance.OnTryToJoinGame?.Invoke(Instance, EventArgs.Empty);
        NetworkManager.Singleton.ConnectionApprovalCallback =Instance.NetwrokManager_ConnectionApprovalCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += Instance.NetwrokManager_OnClientDisconnectCallback;
        
        NetworkManager.Singleton.StartClient();
    }

    private void NetwrokManager_OnClientDisconnectCallback(ulong clientId)
    {
        OnFaildToJoinGame?.Invoke(this, EventArgs.Empty);
    }
    private void NetwrokManager_ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        response.Approved = true;
    }

}
