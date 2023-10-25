using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenEscapeMenu : MonoBehaviour
{
    [SerializeField] private GameObject ui;
    [SerializeField] private GameObject UisGameObjectHolder;
    private bool active;
    // Update is called once per frame

  

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isAnyUiOpen())
        {
            ui.SetActive(!ui.activeSelf);
        }
    }

    private bool isAnyUiOpen()
    {
        bool isAnyUiOpen = false;
        for (int i = 0; i < UisGameObjectHolder.transform.childCount; i++)
        {
            Transform child = UisGameObjectHolder.transform.GetChild(i);
            if (child.childCount > 0 && child != transform)
            {
                isAnyUiOpen = child.GetChild(0).gameObject.activeSelf;
            }
        }
        return isAnyUiOpen;
    }
}
