using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenComputer : MonoBehaviour
{
    [SerializeField] private Sprite PCHighLighted;
    [SerializeField] private Sprite PC;
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
        if (triggerActive)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = PCHighLighted;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = PC;

        }
    }
}
