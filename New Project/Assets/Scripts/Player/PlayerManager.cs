using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using TMPro;
using UnityEditor;
using Unity.VisualScripting;
public class PlayerManager : NetworkBehaviour
{
    private string _username;
    private TextEditorData _textEditor;
    public static event EventHandler<PlayerSpawnArgs> OnAnyPlayerSpawn;
    [SerializeField] private TextMeshProUGUI _usernameText;
    [SerializeField] private GameObject EditorPrefab;


    

    public static PlayerManager LocalPlayer { get; private set; }
    public string GetUsername()
    {
        return _username;
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner) { return; }
        
        LocalPlayer = this;
        SetUsername(UnityEngine.Random.Range(1, 1000000).ToString());

        if (IsHost)
        {
            ComputersManager.ComputersInitialized.OnValueChanged += ComputersInitialized_OnValueChanged;
        }else if (IsClient)
        {
            StartCoroutine(WaitForComputersInitialization());
        }

    }

    private void ComputersInitialized_OnValueChanged(bool previousValue, bool newValue)
    {
        GetEditor();
    }
    private IEnumerator WaitForComputersInitialization()
    {
        yield return new WaitUntil(() => ComputersManager.ComputersInitialized.Value);
        GetEditor();
    }


    private void GetEditor()
    {
        var room = GameObject.Find("Room");
        var computers = room.transform.Find("Computers");

        for (int i = 0; i < computers.childCount; i++)
        {
            var computer = computers.GetChild(i);
            
            if (computer.childCount == 0)
            {
                RequestEditorSpawnServerRpc(i);
                break;
            }

        }
    }


    [ServerRpc]
    private void RequestEditorSpawnServerRpc(int computerId)
    {
        var room = GameObject.Find("Room");
        var computers = room.transform.Find("Computers");
        var computer = computers.GetChild(computerId);

        // Instantiate the text editor for this computer and make it a child of the computer
        GameObject Editor = Instantiate(EditorPrefab);
        NetworkObject networkObjectEditor = Editor.GetComponent<NetworkObject>();
        if (networkObjectEditor != null)
        {
            networkObjectEditor.Spawn();
        }
        Editor.transform.SetParent(computer.transform);
        var thiseditorData = Editor.GetComponent<TextEditorData>();
        thiseditorData.Id = computerId;

        Editor.SetActive(false);

        //assign it to the player
        SetTextEditor(Editor.GetComponent<TextEditorData>());
        thiseditorData.SetOwner(_username);

        print($"the user: {thiseditorData.OwnerUsername} is now a pround owner of {thiseditorData.Id}");
    }
    public void SetUsername(string username)
    {
        _username = username;
    }
    public TextEditorData GetTextEditor()
    {
        return _textEditor;
    }

    public void SetTextEditor(TextEditorData textEditor)
    {
        _textEditor = textEditor;
    }
}
public class PlayerSpawnArgs : EventArgs
{
    public string Username { get; set; }
}
