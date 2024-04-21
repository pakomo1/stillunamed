using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class lobbyCodeManager : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI lobbyCodeText;
    // Start is called before the first frame update
    void Start()
    {
        if (GameLobby.Instance.joinedLobby.IsPrivate)
        {
            lobbyCodeText.text = GameLobby.Instance.joinedLobby.LobbyCode;
            gameObject.SetActive(true);
        }

        else
        {
            print("Lobby is not private");
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
