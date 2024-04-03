using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FetchChangesBtn : MonoBehaviour
{

    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite fetchingSprite;
    [SerializeField] private Image imageToChange;
    [SerializeField] private TextMeshProUGUI changesText;
    [SerializeField] private SingleActiveChild singleActiveChild;
     private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => StartCoroutine(FetchChanges()));
    }

    IEnumerator FetchChanges()
    {
        imageToChange.sprite = fetchingSprite;
        var fetchTask = GitOperations.FetchFromRemoteAsync(GameSceneMetadata.GithubRepoPath,GameSceneMetadata.CurrentBranch);

        yield return new WaitUntil(() => fetchTask.IsCompleted);
        imageToChange.sprite = normalSprite;
        var changes = GitOperations.GetChanges(GameSceneMetadata.GithubRepoPath, GameSceneMetadata.CurrentBranch);

        if (changes.Count > 0)
        {
            changesText.text = $"{changes.Count}";
            singleActiveChild.ActivateOneChild(2);
        }
        print("Fetched");
    }
}
