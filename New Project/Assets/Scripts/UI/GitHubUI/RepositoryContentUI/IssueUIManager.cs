using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IssueUIManager : MonoBehaviour
{
    [SerializeField] private IssuesTemplate issueTemplate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void Show()
    {
        (string ownerName,string repoName) = CreateLobbyUI.GetOwnerAndRepo(GameSceneMetadata.githubRepoLink);
        gameObject.SetActive(true);

        var issues = await GetIssuesForRepository.GetIssuesForRepo(ownerName, repoName);
        issueTemplate.GenerateIssues(issues);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
