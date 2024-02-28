using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyModel : MonoBehaviour
{
    public int Id { get; set; }
    public string OwnerUsername { get; set; }
    public string LobbyName  { get; set; }
    public string GithubLink{ get; set; }
    public int MaxPlayers { get; set; }
}
