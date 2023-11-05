using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class branchesUIManager : MonoBehaviour
{
    [SerializeField] private Button showBranchesUIBtn;
    [SerializeField] private GameObject branchesUI;
    // Start is called before the first frame update
    void Start()
    {
        showBranchesUIBtn.onClick.AddListener(() =>
        {
            branchesUI.SetActive(!branchesUI.activeSelf);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
