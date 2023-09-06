using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RoomCleanUp : MonoBehaviour
{
    private void Awake()
    {
        if(NetworkManager.Singleton != null)
        {
            Destroy(NetworkManager.Singleton.gameObject);
        }
    }
}
