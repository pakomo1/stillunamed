using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class branchesUIManager : MonoBehaviour
{
    [SerializeField] private Button showBranchesUIBtn;
    [SerializeField] private GameObject branchesUI;
    [SerializeField] private GameObject branchContent;
    [SerializeField] private branchesTemplate branchesTemplate;
    // Start is called before the first frame update
    void Start()
    {
        showBranchesUIBtn.onClick.AddListener(() =>
        {
            branchesUI.SetActive(!branchesUI.activeSelf);
        });
        branchesTemplate.OnBranchSelected += BranchesTemplate_OnBranchSelected;
    }

    private void BranchesTemplate_OnBranchSelected(string branchName)
    {
        showBranchesUIBtn.GetComponentInChildren<TextMeshProUGUI>().text = branchName;
    }
}
