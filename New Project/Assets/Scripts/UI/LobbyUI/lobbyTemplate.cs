using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class lobbyTemplate : MonoBehaviour
{
    [SerializeField] private GameObject buttonTemplate;
   public void GenerateLobbie(string lobbyName, string joinedAndMaxPlayersCount)
   {
        GameObject lobbyGameObject = Instantiate(buttonTemplate, transform);
        lobbyGameObject.SetActive(true);

        lobbyGameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = lobbyName;
        lobbyGameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = joinedAndMaxPlayersCount;

        Button lobbuButton = lobbyGameObject.GetComponent<Button>();
        lobbuButton.onClick.AddListener(OnClickLobbyButton);

    }

    private void OnClickLobbyButton()
    {

    }
}
