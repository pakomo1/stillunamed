using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Octokit;
using UnityEngine.UI;

public class IssueUIManager : MonoBehaviour
{
    [SerializeField] private IssuesTemplate issueTemplate;
    [SerializeField] private Button filterButton;
    [SerializeField] private GameObject filterPanel;

    private IssueFilter currentFilter = IssueFilter.All;
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
                    Filter = currentFilter
                };
                print(currentFilter);
                Show(issueRequest);
            });
        }
    }

    
    void Update()
    {
        
    

    }

    public async void Show(RepositoryIssueRequest issueRequest)
    {

        (string ownerName,string repoName) = CreateLobbyUI.GetOwnerAndRepo(GameSceneMetadata.githubRepoLink);
        gameObject.SetActive(true);

        var issues = await GetIssuesForRepository.GetIssuesForRepo(ownerName, repoName,issueRequest);
        issueTemplate.GenerateIssues(issues);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
