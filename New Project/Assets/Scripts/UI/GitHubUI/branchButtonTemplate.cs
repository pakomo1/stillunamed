using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Octokit;
using UnityEngine.UI;

public class branchButtonTemplate : MonoBehaviour
{
    [SerializeField] private GameObject branchButton;
    [SerializeField] private GameObject currentBranchButton;
    private Branch currentBranch;

    public void CreateButtonsForBranches(IReadOnlyList<Branch> branches)
    {
        ClearBranchButtons();
        foreach (var branch in branches)
        {
            var button = Instantiate(branchButton, transform);
            button.SetActive(true);

            TextMeshProUGUI branchNameTextMesh = button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            branchNameTextMesh.overflowMode = TextOverflowModes.Ellipsis;
            branchNameTextMesh.text = branch.Name;

            // Add onClick event to the button
            button.GetComponent<Button>().onClick.AddListener(() => SelectBranch(branch, branchNameTextMesh));
        }
    }

    private void SelectBranch(Branch branch, TextMeshProUGUI branchNameTextMesh)
    {
        currentBranch = branch;
        GameSceneMetadata.CurrentBranch = branch.Name;
        // Update the text of the selected branch button
        TextMeshProUGUI currentBranchTextMesh = currentBranchButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        currentBranchTextMesh.text = branchNameTextMesh.text;
    }
    //clears all the buttons in the branch list
    public void ClearBranchButtons()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
