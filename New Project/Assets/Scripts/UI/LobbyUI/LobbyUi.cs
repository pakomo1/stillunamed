using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUi : MonoBehaviour
{
    [SerializeField] private GameObject connectingUI;
    [SerializeField] private GameLobby gameLobby;
    [SerializeField] private Button createLobbyBtn;
    [SerializeField] private Button refreshButton;

    [SerializeField] private GameObject content;

    [SerializeField] private CreateLobbyUI createLobbyUI;

    public event EventHandler OnTryToJoinGame; 
    public event EventHandler OnFaildToJoinGame;

    private bool refreshComboPressed;

    public static LobbyUi Instance { get; private set; }
    private void Start()
    {
        gameLobby.ListLobbies();
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
        createLobbyBtn.onClick.AddListener(() => 
        {
           createLobbyUI.Show();
        });
        refreshButton.onClick.AddListener(() =>
        {
            for (int i = 0; i < content.transform.childCount; i++)
            {
                Destroy(content.transform.GetChild(i).gameObject);
            }
            gameLobby.ListLobbies();
        });
    }

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            NetworkManager.Singleton.gameObject.SetActive(false);
            Hide();
        }
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
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}