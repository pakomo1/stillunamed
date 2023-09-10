using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class EscapeMenuFunctionalityConnected : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button disconnectButton;

    private void Start()
    {
        resumeButton.onClick.AddListener(OnClickResumeButton);
        optionsButton.onClick.AddListener(OnClickOptionsButton);
        disconnectButton.onClick.AddListener(OnClickDisconnectButton);
    }

    private void OnClickResumeButton()
    {
       transform.GetChild(0).gameObject.SetActive(false);
    }
    private void OnClickOptionsButton()
    {
        //has to open the options panel
    }
    private void OnClickDisconnectButton()
    {
        NetworkManager.Singleton.Shutdown();
        Loader.Load(Loader.Scene.Scene);
    }
}
