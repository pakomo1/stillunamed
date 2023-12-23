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
    // Start is called before the first frame update
    void Start()
    {
        showBranchesUIBtn.onClick.AddListener(() =>
        {

            branchesUI.SetActive(!branchesUI.activeSelf);
        });
    }
    private void Update()
    {
        if (branchesUI.activeSelf)
        {
            GetSelectedButton(branchContent,showBranchesUIBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text).Select();
        }
    }

    private Button GetSelectedButton(GameObject content, string searchText)
    {
        for (int i = 0; i < content.transform.childCount; i++)
        {
            var child = content.transform.GetChild(i);
            TextMeshProUGUI textOfButton = child.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            if (textOfButton.text == searchText)
            {
                return child.GetComponent<Button>();
            }
        }
        return null;
    }
}
