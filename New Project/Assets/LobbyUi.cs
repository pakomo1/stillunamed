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

    public event EventHandler OnTryToJoinGame; 
    public event EventHandler OnFaildToJoinGame;

    public static LobbyUi Instance { get; private set; }
    private void Start()
    {
        connectingUI.SetActive(true);
        createGame.onClick.AddListener(() =>
        {
            StartHost();
            Loader.LoadNetwrok(Loader.Scene.GameScene);
        });
        joinGame.onClick.AddListener(() =>
        {
            StartClient();
        });
    }

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
      
    }

    private void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }
    private void StartClient()
    {
        OnTryToJoinGame?.Invoke(this, EventArgs.Empty);
        NetworkManager.Singleton.ConnectionApprovalCallback = NetwrokManager_ConnectionApprovalCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetwrokManager_OnClientDisconnectCallback;
        
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
