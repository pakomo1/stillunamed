using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class EscapeMenuFunctionalityConnected : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button disconnectButton;

    public event EventHandler onPlayerDisconect;


    public static EscapeMenuFunctionalityConnected Instance { get; private set; }

    private void Start()
    {
        resumeButton.onClick.AddListener(OnClickResumeButton);
        optionsButton.onClick.AddListener(OnClickOptionsButton);
        disconnectButton.onClick.AddListener(OnClickDisconnectButton);
    }


    private void Awake()
    {
        Instance = this;
    }
    private void OnClickResumeButton()
    {
       transform.GetChild(0).gameObject.SetActive(false);
    }
    private void OnClickOptionsButton()
    {
        //has to open the options panel
    }
    private void OnClickDisconnectButton()
    {
        onPlayerDisconect.Invoke(this, EventArgs.Empty);
        NetworkManager.Singleton.Shutdown();
        Loader.Load(Loader.Scene.Scene);

        if (GameLobby.Instance.isLobbyHost())
        {
            GameLobby.Instance.ShutDownLobby(GameLobby.Instance.joinedLobby.Id);
        }
        
    }
}
