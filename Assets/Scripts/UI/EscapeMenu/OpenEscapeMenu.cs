using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenEscapeMenu : MonoBehaviour
{
    [SerializeField] private GameObject ui;
    [SerializeField] private GameObject UisGameObjectHolder;
    [SerializeField] private CloseAndOpenAnotherUi closeAndOpenAnotherUi;
    private bool active;
    // Update is called once per frame

  

    void Update()
    {
        var objOnTop = closeAndOpenAnotherUi.FindTheGameObjectOnTop(UisGameObjectHolder);
        if (Input.GetKeyDown(KeyCode.Escape) && objOnTop == null)
        {
            ui.SetActive(!ui.activeSelf);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && objOnTop != null)
        {
           GameObject.Find(objOnTop.GetComponentInParent<Transform>().name).SetActive(false);
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
