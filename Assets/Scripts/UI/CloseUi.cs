using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUi : MonoBehaviour
{
    [SerializeField] private GameObject UI;

    public void OnClickCloseUI()
    {
        UI.SetActive(false);
    }
}
