using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideBarManagerUI : MonoBehaviour
{
    [SerializeField] private GameObject repositoryContentUI;
    [SerializeField] private Button showSideBarButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (repositoryContentUI.activeSelf)
        {
            gameObject.SetActive(true);
            showSideBarButton.gameObject.SetActive(true);
        }
        else
        {
            gameObject.gameObject.SetActive(false);
            showSideBarButton.gameObject.SetActive(false);
        }
    }
}
