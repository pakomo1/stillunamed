using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class OpenInteraction : MonoBehaviour
{
    //[SerializeField] private SpriteRenderer sprite;
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private CameraTarget cameraTarget;
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
            dialogueUI.SetActive(true);
        }
        if (dialogueUI.activeInHierarchy)
        {
            cameraTarget.target = cameraTarget.guideBotTransform;
            cameraTarget.threshold = 0.6f;
        }
        else
        {
            try
            {
                cameraTarget.target = cameraTarget.playerTransform;
                cameraTarget.threshold = 0.2f;
            }
            catch
            {
                
            }
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
