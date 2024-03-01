using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserModel
{
    public string Username;
    public Dictionary<string, bool> Lobbies;

    public UserModel(string username)
    {
        Username = username;
        Lobbies = new Dictionary<string, bool>();
    }
}
