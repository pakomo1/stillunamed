using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInteraction : MonoBehaviour
{
    //[SerializeField] private SpriteRenderer sprite;
    private bool triggerActive;

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
            print(true);
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
