using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LobbyModel
{
    public int Id { get; set; }
    public string OwnerUsername { get; set; }
    public string LobbyName  { get; set; }
    public string GithubLink{ get; set; }
    public int MaxPlayers { get; set; }
}
