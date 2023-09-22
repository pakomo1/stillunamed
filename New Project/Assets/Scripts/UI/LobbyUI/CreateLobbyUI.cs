using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Octokit;

public class CreateLobbyUI : MonoBehaviour
{
    [SerializeField] private Button createButton;
    [SerializeField] private TMP_InputField lobbyName;
    [SerializeField] private TMP_InputField repoLink;
    [SerializeField] private TMP_InputField maxPlayerCount;
    [SerializeField] private Toggle isPrivate;
    
    [SerializeField] private GameLobby gameLobby;

    private void Start()
    {
        createButton.onClick.AddListener(() =>
        {
            gameLobby.CreateLobby(lobbyName.text, repoLink.text, isPrivate.isOn, int.Parse(maxPlayerCount.text));
            lobbyName.text = "";
            repoLink.text = "";
            maxPlayerCount.text = "0";

            Hide();
        });

        if (Input.GetKey(KeyCode.Escape))
        {
            Hide();
        }
    }
    private void CheckIfRepoLinkIsReal(string url)
    {
        // check if the link is real by making a request to the github api 
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
