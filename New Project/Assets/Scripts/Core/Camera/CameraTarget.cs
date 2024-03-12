using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] public Transform playerTransform;
    [SerializeField] public Transform guideBotTransform;
    [SerializeField] public float threshold;
    public Transform target;

    private void Awake()
    {
        target = playerTransform;
    }

    private void Update()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPos;
        if (target == guideBotTransform)
        {
            targetPos = (playerTransform.position + guideBotTransform.position) / 2f;
        }
        else
        {
            targetPos = (target.position + mousePos) / 2f;
        }

        targetPos.x = Mathf.Clamp(targetPos.x, -threshold + target.position.x, threshold + target.position.x);
        targetPos.y = Mathf.Clamp(targetPos.y, -threshold + target.position.y, threshold + target.position.y);

        transform.localPosition = targetPos;
    }
}
