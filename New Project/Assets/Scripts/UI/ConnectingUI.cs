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
    [SerializeField] private TextMeshProUGUI message;
    [SerializeField] private Button closeButton; 
    void Start()
    {
        Hide();
        LobbyUi.Instance.OnTryToJoinGame += LobbyUI_OnTryToJoinGame;
        LobbyUi.Instance.OnFaildToJoinGame += LobbyUI_OnFaildToJoinGame;
    }

    private void LobbyUI_OnFaildToJoinGame(object sender, System.EventArgs e)
    {
        print("Client faild to join game");
        message.text = NetworkManager.Singleton.DisconnectReason;
        if(message.text == "")
        {
            message.text = "Failed to connect";
        }
        closeButton.gameObject.SetActive(true);
        Show();
    }

    private void LobbyUI_OnTryToJoinGame(object sender, System.EventArgs e)
    {
        message.text = "Connecting...";
        closeButton.gameObject.SetActive(false);
        Show();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        LobbyUi.Instance.OnFaildToJoinGame -= LobbyUI_OnFaildToJoinGame;
        LobbyUi.Instance.OnTryToJoinGame -= LobbyUI_OnTryToJoinGame;
    }
}
