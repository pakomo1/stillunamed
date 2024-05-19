using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class lobbyTemplate : MonoBehaviour
{
    [SerializeField] private GameObject buttonTemplate;
    [SerializeField] private GameLobby gameLobby;
   public void GenerateLobbie(Lobby lobby)
   {
        string joinedPlyersCountAndMaxPlayers = $"{lobby.Players.Count}/{lobby.MaxPlayers}";


        GameObject lobbyGameObject = Instantiate(buttonTemplate, transform);
        lobbyGameObject.SetActive(true);

        string repoLink = lobby.Data["repoLink"].Value;

        lobbyGameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = lobby.Name;
        lobbyGameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = joinedPlyersCountAndMaxPlayers;
        lobbyGameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = repoLink;


        Button lobbuButton = lobbyGameObject.GetComponent<Button>();
        lobbuButton.onClick.AddListener(()=> { OnClickLobbyButton(lobby); });

    }

    private  void OnClickLobbyButton(Lobby lobby)
    {
        gameLobby.JoinLobbyByID(lobby.Id);
        gameLobby.PrintPlayersInLobby(lobby);
    }
}
