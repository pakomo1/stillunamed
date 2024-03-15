using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
public class PlayerManager : NetworkBehaviour
{
    private string _username;
    private TextEditorData _textEditor;
    public static event EventHandler<PlayerSpawnArgs> OnAnyPlayerSpawn;

    public static PlayerManager LocalPlayer { get; private set; }
    public string GetUsername()
    {
        return _username;
    }
    public override void OnNetworkSpawn()
    {
        print("Player Spawned");
        if (IsOwner)
        {
            LocalPlayer = this;
            SetUsername(UnityEngine.Random.Range(1, 1000000).ToString());
            OnAnyPlayerSpawn?.Invoke(this, new PlayerSpawnArgs { Username = GetUsername() });
        }
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
