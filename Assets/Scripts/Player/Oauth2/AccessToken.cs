using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AccessToken : NetworkBehaviour
{
    [SerializeField] public string accessToken;

    private void Update()
    {
        if (!IsOwner) accessToken = null;
    }
}
