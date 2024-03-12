using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideBarManagerUI : MonoBehaviour
{
    [SerializeField] private GameObject repositoryContentUI;
    [SerializeField] private Button showSideBarButton;

    public void Show()
    {
        gameObject.SetActive(true);
        showSideBarButton.gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
        showSideBarButton.gameObject.SetActive(false);
    }
}
