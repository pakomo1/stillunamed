using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Octokit;
using UnityEngine.UI;
using System.Linq;

public class IssueUIManager : MonoBehaviour
{
    [SerializeField] private IssuesTemplate issueTemplate;
    [SerializeField] private Button filterButton;
    [SerializeField] private GameObject filterPanel;

    private IssueFilter currentFilter = IssueFilter.Created;
    private Dictionary<string, IssueFilter> filters= new Dictionary<string, IssueFilter>()
    {
        {"YourIssuesFilterButton",IssueFilter.Created},
        {"AssignedToYouFilterButton",IssueFilter.Assigned},
        {"MentionedYouFilterButton",IssueFilter.Mentioned},
    };
    void Start()
    {
        filterButton.onClick.AddListener(() =>
        {
            filterPanel.SetActive(!filterPanel.activeSelf);
        });
        for (int i = 0; i < filterPanel.transform.childCount; i++)
        {
            var filterButton = filterPanel.transform.GetChild(i).GetComponent<Button>();
            filterButton.onClick.AddListener(() =>
            {
                currentFilter = filters[filterButton.name];
                RepositoryIssueRequest issueRequest = new RepositoryIssueRequest()
                {
                    Filter = IssueFilter.Created
                };
                print(currentFilter);
                Generate(issueRequest);
            });
        }
    }

    
    void Update()
    {

    }

    public async void Generate(RepositoryIssueRequest issueRequest)
    {
        (string ownerName,string repoName) = GitHelperMethods.GetOwnerAndRepo(GameSceneMetadata.GithubRepoLink);
        var issues = await GetIssuesForRepository.GetIssuesForRepo(ownerName, repoName, issueRequest);
        issueTemplate.GenerateIssues(issues);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
