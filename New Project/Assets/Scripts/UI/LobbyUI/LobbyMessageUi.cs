using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ConnectingUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject lobbyMessageUI; 
    [SerializeField] private TextMeshProUGUI message;
    [SerializeField] private Button closeButton; 
    void Start()
    {
        Hide();
        //lobby event listeners
        LobbyUi.Instance.OnTryToJoinGame += LobbyUI_OnTryToJoinGame;
        LobbyUi.Instance.OnFaildToJoinGame += LobbyUI_OnFaildToJoinGame;

        GameLobby.Instance.OnCreateLobbyStarted += GameLobby_OnCreateLobbyStarted;
        GameLobby.Instance.OnCreateLobbyFailed += GameLobby_OnCreateLobbyFailed;

        GameLobby.Instance.OnLobbyJoinStarted += GameLobby_OnLobbyJoinStarted;
        GameLobby.Instance.OnLobbyJoinFailed += GameLobby_OnLobbyJoinFailed;

        //git event listeners
        GameLobby.Instance.OnForkStarted += GameLobby_OnForkStarted;
        GameLobby.Instance.OnForkFailed += GameLobby_OnForkFailed;
        GameLobby.Instance.OnForkSuccess += GameLobby_OnForkSuccess;

        GameLobby.Instance.OnCloneStarted += GameLobby_OnCloneStarted;
        GameLobby.Instance.OnCloneFailed += GameLobby_OnCloneFailed;
        GameLobby.Instance.OnCloneSuccess += GameLobby_OnCloneSuccess;

    }

    private void GameLobby_OnCloneSuccess(object sender, EventArgs e)
    {
        ShowMessage("Cloning repository success", true);
    }

    private void GameLobby_OnCloneFailed(object sender, GitOperationsEventArgs e)
    {
        ShowMessage(e.ErrorMessage, true);
    }

    private void GameLobby_OnCloneStarted(object sender, EventArgs e)
    {
        ShowMessage("Cloning repository...", false);
    }

    private void GameLobby_OnForkSuccess(object sender, EventArgs e)
    {
        ShowMessage("Forking repository success", true);
    }

    private void GameLobby_OnForkFailed(object sender, GitOperationsEventArgs e)
    {
        ShowMessage(e.ErrorMessage, true);
    }

    private void GameLobby_OnForkStarted(object sender, EventArgs e)
    {
        ShowMessage("Forking repository...", false);
    }

    private void GameLobby_OnLobbyJoinFailed(object sender, EventArgs e)
    {
        ShowMessage("Failed to join lobby", true);
    }

    private void GameLobby_OnLobbyJoinStarted(object sender, EventArgs e)
    {
        ShowMessage("Joining lobby...", false);
    }

    private void GameLobby_OnCreateLobbyFailed(object sender, EventArgs e)
    {
        ShowMessage("Failed to create lobby", true);
    }

    private void GameLobby_OnCreateLobbyStarted(object sender, EventArgs e)
    {
        ShowMessage("Creating lobby...", false);
    }

    private void LobbyUI_OnFaildToJoinGame(object sender, System.EventArgs e)
    {
        print("Client faild to join game");
        if (NetworkManager.Singleton.DisconnectReason == "")
        {
            ShowMessage("Failed to connect", true);
        }
        else
        {
            ShowMessage(NetworkManager.Singleton.DisconnectReason, true);
        }
    }

    private void LobbyUI_OnTryToJoinGame(object sender, System.EventArgs e)
    {
        message.text = "Connecting...";
        closeButton.gameObject.SetActive(false);
        Show();
    }

    private void ShowMessage(string messagetoDisplay, bool withCloseButton)
    {
        Show();
        if (withCloseButton)
        {
            closeButton.gameObject.SetActive(true);
        }
        else
        {
            closeButton.gameObject.SetActive(false);
        }
        message.text = messagetoDisplay;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void Show()
    {
        lobbyMessageUI.SetActive(true);
    }
    private void Hide()
    {
        lobbyMessageUI.SetActive(false);
    }
    private void OnDestroy()
    {
        //LobbyUi.Instance.OnFaildToJoinGame -= LobbyUI_OnFaildToJoinGame;
        //LobbyUi.Instance.OnTryToJoinGame -= LobbyUI_OnTryToJoinGame;
    }
}
