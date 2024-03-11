using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabBetweenUI1 : MonoBehaviour
{
    [SerializeField] private GameObject contentHolder;
    [SerializeField] private GameObject uiToSetActive;

    public void SetActive()
    {
        ActivateObjectInContent.OnClickSwitchToThisUI(contentHolder, uiToSetActive);
    }
}
