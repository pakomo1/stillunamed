using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneMessageUi : MonoBehaviour
{
    [SerializeField] private GameObject MessageUiPanel;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button closeButton;
    // Start is called before the first frame update
    void Start()
    {
        EscapeMenuFunctionalityConnected.Instance.onPlayerDisconect += EscapeMenu_onPlayerDisconect;
    }

    private void EscapeMenu_onPlayerDisconect(object sender, System.EventArgs e)
    {
        ShowMessage("Disconneting...", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void ShowMessage(string message,bool withCloseBtn)
    {
        if (withCloseBtn)
        {
            closeButton.gameObject.SetActive(true);
        }
        else
        {
            closeButton.gameObject.SetActive(false);
        }
        messageText.text = message;
        Show();
    }
    private void Show()
    {
        MessageUiPanel.gameObject.SetActive(true);
    }
    private void Hide()
    {
        MessageUiPanel.gameObject.SetActive(false);
    }
}
