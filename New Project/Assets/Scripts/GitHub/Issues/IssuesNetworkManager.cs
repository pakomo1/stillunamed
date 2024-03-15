using Octokit;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class IssuesNetworkManager : NetworkBehaviour
{
    public NetworkList<SerializedIssues> issues;


    private void Awake()
    {
        issues = new NetworkList<SerializedIssues>();
    }
    public async Task GetIssuesForRepoAsync(string ownerName, string repositoryName,RepositoryIssueRequest issueRequest)
   {
        var issues = await GetIssuesForRepository.GetIssuesForRepo(ownerName, repositoryName, issueRequest);
        this.issues.Clear();
        foreach (var issue in issues)
        {
            this.issues.Add(new SerializedIssues
            {
                Id = issue.Id,
                NodeId = issue.NodeId ?? string.Empty,
                Url = issue.Url ?? string.Empty,
                HtmlUrl = issue.HtmlUrl ?? string.Empty,
                CommentsUrl = issue.CommentsUrl ?? string.Empty,
                EventsUrl = issue.EventsUrl ?? string.Empty,
                Number = issue.Number,
                State = issue.State.ToString(),
                Title = issue.Title ?? string.Empty,
                Body = issue.Body ?? string.Empty,
                ClosedAt = issue.ClosedAt,
                CreatedAt = issue.CreatedAt,
                UpdatedAt = issue.UpdatedAt,
                Locked = issue.Locked,
                Comments = issue.Comments
            });
        }
    }
}
