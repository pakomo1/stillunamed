using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class OnEdgeScrollCameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject cinemachineGameObject;
    private CinemachineVirtualCamera mCam;
    private CinemachineFramingTransposer mFramingTransposer;
    [SerializeField] private Transform player;
    [SerializeField] public float threshold;

    void Start()
    {
        mCam = cinemachineGameObject.GetComponent<CinemachineVirtualCamera>();
        mFramingTransposer = mCam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 targetPos = (player.position + mousePos) / 2f;

        targetPos.x = Mathf.Clamp(targetPos.x, -threshold + player.position.x, threshold + player.position.x);
        targetPos.y = Mathf.Clamp(targetPos.y, -threshold + player.position.y, threshold + player.position.y);

        mFramingTransposer.m_ScreenX = targetPos.x;
        mFramingTransposer.m_ScreenY = targetPos.y; 
    }
}
