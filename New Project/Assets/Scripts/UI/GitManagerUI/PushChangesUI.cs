using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PushChangesUI : MonoBehaviour
{
    [SerializeField]private GitManager gitManager;
    [SerializeField] private GameObject commitsCountHolder;
    [SerializeField]private TextMeshProUGUI unpushedCommitsCountLbl;
    private string branch;
    private int commitsCount;
    // Start is called before the first frame update
    void Start()
    {
        branch = "main";
        gitManager.onChangesCommitted += GitManager_onChangesCommitted;
        commitsCount = GitOperations.CountUnpushedCommits(GameSceneMetadata.githubRepoPath,branch);
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
        commitsCount = GitOperations.CountUnpushedCommits(GameSceneMetadata.githubRepoPath,branch);
        if (commitsCount > 0)
        {
            commitsCountHolder.SetActive(true);
        }   
        unpushedCommitsCountLbl.text = commitsCount.ToString();
    }
    public void PushChanges()
    {
        GitOperations.PushChanges(GameSceneMetadata.githubRepoPath, branch);
        commitsCountHolder.SetActive(false);
        print("Changes have been pushed to the origin");
    }   

    // Update is called once per frame
    void Update()
    {
        
    }
}
