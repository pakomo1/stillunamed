using System;
using Unity.Netcode;
using UnityEngine;

public class OpenComputer : NetworkBehaviour
{
    [SerializeField] private Sprite PCHighLighted;
    [SerializeField] private Sprite PC;
    private TextEditorData textEditorData;
    private PlayerManager playerInTrigger;
    private TextEditorManager textEditorManager;
    private bool triggerActive;

    private bool doesExist = false; 

    private void Awake()
    {
        triggerActive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            triggerActive = true;
            //print(textEditorData.OwnerUsername);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            triggerActive = false;
        }
    }
    private void Start()
    {
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        var ui = GameObject.Find("UI");
        var textEditorCanvas = ui.transform.Find("TextEditorCanvas");   
        textEditorManager = textEditorCanvas.GetComponent<TextEditorManager>();

        PlayerManager.OnEditorSpawned += PlayerManager_OnEditorSpawned;
    }

    private void PlayerManager_OnEditorSpawned(object sender, EventArgs e)
    {
       if(transform.childCount > 0)
       {
           
       }
       else
       {
            doesExist = false;
       }
    }

    private void Update()
    {
        if (!IsClient) { return;}
        //Check if interaction is available
        if (triggerActive && Input.GetKeyDown(KeyCode.E))
        {
            doesExist = transform.GetChild(0).TryGetComponent<TextEditorData>(out TextEditorData data);
            if (doesExist)
            {
                if (data.OwnerUsername != PlayerManager.LocalPlayer.GetUsername())
                {
                    print("This computer is currently in use by another player.");
                    return;
                }
              textEditorData = data;
              textEditorManager.LoadEditorData(textEditorData);
            }
            else
            {
                print("There is no text editor in this pc");
            }
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
