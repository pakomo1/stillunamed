using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnFromSideBar : MonoBehaviour
{
    [SerializeField] private GameObject moveToSideBarGrid;

    public void OnMouseEnter()
    {
        print("Returned");
        gameObject.SetActive(false);
        moveToSideBarGrid.SetActive(true);
    }
}
