using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Octokit;
using System.Threading.Tasks;

public class GetIssuesForRepository : MonoBehaviour
{
    public static async Task<IReadOnlyList<Issue>> GetIssuesForRepo(string owner, string repoName)
    {
        var issues = await GitHubClientProvider.client.Issue.GetAllForRepository(owner, repoName);
        return issues;  
    }
}
