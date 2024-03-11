using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class IndividualCameraController : MonoBehaviour
{
    [SerializeField] private new GameObject camera;

    private void Update()
    {
        //if (!IsOwner) return;
        camera.SetActive(true);
    }
}
