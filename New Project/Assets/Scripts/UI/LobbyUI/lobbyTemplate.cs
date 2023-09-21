using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class lobbyTemplate : MonoBehaviour
{
    [SerializeField] private GameObject buttonTemplate;
   public void GenerateLobbie(string lobbyName, int playerCount)
   {
        var button = Instantiate(buttonTemplate, transform);
        button.SetActive(true);

        button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = lobbyName;
        button.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = playerCount.ToString();
    }
}
