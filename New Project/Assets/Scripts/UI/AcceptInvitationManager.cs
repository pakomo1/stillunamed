using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AcceptInvitationManager : MonoBehaviour
{
    [SerializeField] private Button closeBtn;
    // Start is called before the first frame update
    void Start()
    {
        closeBtn.onClick.AddListener(Close);  
    }
    //hides the gameobject
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
