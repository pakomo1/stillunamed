using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PullChangesBtn : MonoBehaviour
{
    private Button button; // The button component
    [SerializeField]private SingleActiveChild singleActiveChild; 

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => StartCoroutine(PullChanges()));
    }

    IEnumerator PullChanges()
    {
        var pullTask = GitOperations.PullChangesAsync(GameSceneMetadata.GithubRepoPath,GameSceneMetadata.CurrentBranch);
        yield return new WaitUntil(() => pullTask.IsCompleted);
        singleActiveChild.ActivateOneChild(1);
    }
}
