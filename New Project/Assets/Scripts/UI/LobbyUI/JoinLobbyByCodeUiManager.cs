using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class JoinLobbyByCodeUiManager : MonoBehaviour
{
    [SerializeField]private TMP_InputField lobbyCodeInputField;
    [SerializeField] private Button JoinLobbyBtn;

    private void Start()
    {
        JoinLobbyBtn.onClick.AddListener(JoinLobby);
    }

    private void JoinLobby()
    {
        string lobbyCode = lobbyCodeInputField.text;
        if (string.IsNullOrEmpty(lobbyCode))
        {
            Debug.LogError("Lobby code is empty");
            return;
        }
        GameLobby.Instance.JoinPrivateLobby(lobbyCode);
    }
   public void Show()
    {
        gameObject.SetActive(true);
    }
}
