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
    private int changes;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => StartCoroutine(FetchChanges()));
    }

    IEnumerator FetchChanges()
    {
        imageToChange.sprite = fetchingSprite;
        var fetchTask = GitOperations.FetchFromRemoteAsync(GameSceneMetadata.GithubRepoPath,GameSceneMetadata.CurrentBranch);

        // Add a small delay
        yield return new WaitForSeconds(1);

        yield return new WaitUntil(() => fetchTask.IsCompleted);
        imageToChange.sprite = normalSprite;
      
        GetChanges();
        if (changes > 0)
        {
            changesText.text = $"{changes}";
            singleActiveChild.ActivateOneChild(2);
        }
        print("Fetched");
    }
    private async void GetChanges()
    {
        changes = await GitOperations.GetChanges(GameSceneMetadata.GithubRepoPath, GameSceneMetadata.CurrentBranch);
    }   
}
