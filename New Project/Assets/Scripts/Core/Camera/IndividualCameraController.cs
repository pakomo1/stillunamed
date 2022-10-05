using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class IndividualCameraController : NetworkBehaviour
{
    [SerializeField] private GameObject camera;

    private void Update()
    {
        if (!IsOwner) return;
        camera.active = true;
    }
}
