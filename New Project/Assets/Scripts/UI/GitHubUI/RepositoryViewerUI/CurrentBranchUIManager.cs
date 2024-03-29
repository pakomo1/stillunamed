using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Octokit;
public class CurrentBranchUIManager : MonoBehaviour
{
    [SerializeField] private Button currentBranchButton;
    [SerializeField] private GameObject branchList;
    [SerializeField] private branchButtonTemplate branchButtonTemplate;
    // Start is called before the first frame update
    void Start()
    {
        currentBranchButton.onClick.AddListener(() =>
        {
            branchList.SetActive(!branchList.activeSelf);
            GenerateBranches();
        }); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Generate branches
    public async void GenerateBranches()
    {
        string[] ownerandrepo = GameSceneMetadata.GetOwnerAndRepoName();
        string owner = ownerandrepo[0];
        string repo = ownerandrepo[1];

        //gets all branches of the repository
        var branches = await GitHubClientProvider.client.Repository.Branch.GetAll(owner, repo);
        branchButtonTemplate.CreateButtonsForBranches(branches);
    }
}
