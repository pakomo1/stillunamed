using Octokit;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUi : MonoBehaviour
{
    [SerializeField] private GameObject menu;

    [SerializeField] private GameObject connectingUI;
    [SerializeField] private GameLobby gameLobby;
    [SerializeField] private Button createLobbyBtn;
    [SerializeField] private Button refreshButton;
    [SerializeField] private Button CloseButton;

    [SerializeField] private GameObject content;

    [SerializeField] private CreateLobbyUI createLobbyUI;

    public event EventHandler OnTryToJoinGame; 
    public event EventHandler OnFaildToJoinGame;


    public static LobbyUi Instance { get; private set; }
    private void Start()
    {
        gameLobby.ListLobbies();
        
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

        /*CloseButton.onClick.AddListener(() =>
        {
            menu.SetActive(false);
            NetworkManager.Singleton.gameObject.SetActive(false);

        });*/
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
        NetworkManager.Singleton.OnClientDisconnectCallback += Instance.NetwrokManager_OnClientDisconnectCallback;
        
        NetworkManager.Singleton.StartClient();
    }

    private void NetwrokManager_OnClientDisconnectCallback(ulong clientId)
    {
        OnFaildToJoinGame?.Invoke(this, EventArgs.Empty);
    }
    public void Show()
    {
       gameObject.SetActive(true);
    }
    public void Hide()
    {
        menu.gameObject.SetActive(false);
    }
}
