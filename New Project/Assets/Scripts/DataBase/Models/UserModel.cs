using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserModel
{
    public string Username;
    public List<int> recentLobbies;

    public UserModel(string username)
    {
        Username = username;
    }
}
