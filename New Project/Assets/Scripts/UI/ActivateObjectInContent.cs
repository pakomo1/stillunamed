using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActivateObjectInContent
{
    private static List<GameObject> elements = new List<GameObject>();

    public static void OnClickSwitchToThisUI(GameObject contentHolder, GameObject uiToSetActive)
    {
        elements = GetGameObjectContents.GetContent(contentHolder);

        foreach (var item in elements)
        {
            if (item.activeSelf)
            {
                item.SetActive(false);
            }
        }
        uiToSetActive.SetActive(true);
        elements = GetGameObjectContents.GetContent(uiToSetActive);
        foreach (var item in elements)
        {
            if (!item.activeSelf)
            {
                item.SetActive(true);
            }
        }
    }
}
