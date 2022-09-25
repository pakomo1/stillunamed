using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float threshold;

    private void Update()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPos = (playerTransform.position + mousePos) / 2;

        targetPos.x = Mathf.Clamp(targetPos.x, -threshold + playerTransform.position.x, threshold + playerTransform.position.x);
        targetPos.y = Mathf.Clamp(targetPos.y, -threshold + playerTransform.position.y, threshold + playerTransform.position.y);

        transform.localPosition = targetPos;
    }
}
