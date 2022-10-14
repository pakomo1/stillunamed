using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseAndOpenAnotherUi : MonoBehaviour
{
    [SerializeField] private GameObject uiToClose;
    [SerializeField] private GameObject uiToOpen;

    public void OnClick()
    {
        uiToClose.SetActive(false);
        uiToOpen.SetActive(true);
    }
}
