using LibGit2Sharp;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PushChangesUI : MonoBehaviour
{
    [SerializeField]private GitManager gitManager;
    [SerializeField]private GameObject commitsCountHolder;
    [SerializeField]private GameObject changedFilesContent;
    [SerializeField] private TextMeshProUGUI changedFilesCount;
    [SerializeField]private TextMeshProUGUI unpushedCommitsCountLbl;
    private string branch;
    private int commitsCount;
    // Start is called before the first frame update
    void Start()
    {
        branch = "main";
        gitManager.onChangesCommitted += GitManager_onChangesCommitted;
        commitsCount = GitOperations.CountUnpushedCommits(GameSceneMetadata.GithubRepoPath,branch);
        if (commitsCount > 0)
        {
            commitsCountHolder.SetActive(true);
            unpushedCommitsCountLbl.text = commitsCount.ToString(); 
        }
        else
        {
            commitsCountHolder.SetActive(false);
        }
    }

    private void GitManager_onChangesCommitted(object sender, EventArgs e)
    {
        //should add the proper branch
        commitsCount = GitOperations.CountUnpushedCommits(GameSceneMetadata.GithubRepoPath,branch);
        if (commitsCount > 0)
        {
            commitsCountHolder.SetActive(true);
        }   
        unpushedCommitsCountLbl.text = commitsCount.ToString();
    }
    public async void PushChanges()
    {
        try
        {
            await GitOperations.PushChangesAsync(GameSceneMetadata.GithubRepoPath, branch);
            commitsCountHolder.SetActive(false);
            print("Changes have been pushed to the origin");
        }
        catch(Exception ex)
        {
            Debug.LogError(ex);
        }
     
    }   

    // Update is called once per frame
    void Update()
    {
        
    }
    //clears the content of the changed files
 
}