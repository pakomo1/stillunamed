using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLobbyExplorer : MonoBehaviour
{
    private bool triggerActive;
    [SerializeField] private GameObject lobbyExplorer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            triggerActive = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {   
        if(collision.tag == "Player")
        {
            triggerActive = false;  
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(triggerActive && Input.GetKeyDown(KeyCode.E))
        {
            lobbyExplorer.SetActive(true); 
        }
    }
}
