using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;
using Unity.VisualScripting;
using LibGit2Sharp;
public class CommitChangesUI : MonoBehaviour
{
    [SerializeField]private TMP_InputField commitMessageInput;
    [SerializeField]private TMP_InputField commitDescription;
    [SerializeField]private GameObject changedFilesContent;
    [SerializeField]private TextMeshProUGUI changesCount;
    [SerializeField]private Button commitButton;
    [SerializeField]private GitManager gitManager;

    [SerializeField]private SingleActiveChild singleActiveChild;
    // Start is called before the first frame update
    void Start()
    {
        commitButton.onClick.AddListener(CommitChangesAsync);
    }

    private async void CommitChangesAsync()
    {
        try
        {
            await gitManager.CommitChangesAsync(GameSceneMetadata.GithubRepoPath, commitMessageInput.text, commitDescription.text,GameSceneMetadata.CurrentBranch);
            commitMessageInput.text = "";
            commitDescription.text = "";
           ClearChangedFiles();

            //change to the push button
            singleActiveChild.ActivateOneChild(0);
        }
        catch (Exception ex)
        {
            // Handle any errors that occur during the commit
            Debug.LogError($"Commit failed: {ex.Message}");
        }
    }
    public void ClearChangedFiles()
    {
        foreach (Transform child in changedFilesContent.transform)
        {
            Destroy(child.gameObject);
        }
        using (var repo = new Repository(GameSceneMetadata.GithubRepoPath))
        {
            var changes = repo.Diff.Compare<TreeChanges>(repo.Head.Tip.Tree, DiffTargets.Index | DiffTargets.WorkingDirectory);
            changesCount.text = "Changed Files: " + changes.Count;
        }
    }
    // Update is called once per frame
    void Update()
    {
        commitButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Commit to {GameSceneMetadata.CurrentBranch}";
    }
}
