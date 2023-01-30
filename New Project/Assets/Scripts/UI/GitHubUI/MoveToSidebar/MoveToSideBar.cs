using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveToSideBar : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private GameObject objectToTurnOn;
    public Vector3 mousePosition;
    public float leftBoundary;

    public void OnPointerEnter(PointerEventData eventData)
    {
        print(true);
    }

    public void Yes()
    {
        print(true);
    }
}
