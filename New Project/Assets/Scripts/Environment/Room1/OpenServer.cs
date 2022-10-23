using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenServer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private GameObject authorizationUi;
    [SerializeField] private GameObject gitHubUi;
    private bool triggerActive;
    private GameObject player;
    public bool authorized;

    private void Awake()
    {
        triggerActive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            triggerActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            triggerActive = false;
        }
    }

    private void Update()
    {
        //Check if interaction is available
        if (triggerActive && Input.GetKeyDown(KeyCode.E))
        {
            if (!authorized)
            {
                authorizationUi.SetActive(true);
            }
            else
            {
                gitHubUi.SetActive(true);
            }
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (authorizationUi.activeSelf || gitHubUi.activeSelf)
        {
            player.transform.GetChild(0).GetComponent<PlayerMovement>().movementDirection = Vector2.zero;
        }

        //If interaction is available highlight the object
        /*if (triggerActive)
        {
            sprite.enabled = true;
        }
        else
        {
            sprite.enabled = false;
        }*/
    }
}
