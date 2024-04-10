using System;
using Unity.Netcode;
using UnityEngine;

public class OpenComputer : NetworkBehaviour
{
    [SerializeField] private Sprite PCHighLighted;
    [SerializeField] private Sprite PC;
    private TextEditorData textEditorData;
    private TextEditorData currentTextEditorData;
    private PlayerManager currentPlayer;
    private TextEditorManager textEditorManager;
    private bool triggerActive;
    private int playersInTrigger = 0;
    private bool doesExist = false; 

    private void Awake()
    {
        triggerActive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playersInTrigger++;
            // Only update triggerActive and currentTextEditorData if no player is currently interacting with the computer
            if (currentPlayer == null)
            {
                triggerActive = true;
                currentPlayer = collision.GetComponent<PlayerManager>();
                if (transform.childCount == 1)
                {
                   currentTextEditorData = transform.GetChild(0).GetComponent<TextEditorData>();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playersInTrigger--;
            if (collision.GetComponent<PlayerManager>() == currentPlayer)
            {
                triggerActive = false;
                currentPlayer = null;
            }
        }
    }
    private void Start()
    {

    }
    //TODO: when the client tries to connect it cant find the textEditorManager.Instance
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        textEditorManager = TextEditorManager.Instance;
    }

    

    private void Update()
    {
        if (!IsClient) { return;}
        //Check if interaction is available
        if (triggerActive && Input.GetKeyDown(KeyCode.E) && currentPlayer == PlayerManager.LocalPlayer && !PlayerManager.LocalPlayer.isPlayerInteracting())
        {
            //doesExist = transform.GetChild(0).TryGetComponent<TextEditorData>(out TextEditorData data);
            if (currentTextEditorData != null)
            {
                PlayerManager.LocalPlayer.StartInteractingWithUI();
                textEditorManager.LoadEditorData(currentTextEditorData); 
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
