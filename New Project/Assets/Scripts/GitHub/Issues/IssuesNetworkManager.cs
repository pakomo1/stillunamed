using Octokit;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class IssuesNetworkManager : NetworkBehaviour
{
    public NetworkVariable<List<Issue>> issues;


    private void Awake()
    {
        issues = new NetworkVariable<List<Issue>>(new List<Issue>(), NetworkVariableReadPermission.Everyone);
    }
    public async Task GetIssuesForRepoAsync(string ownerName, string repositoryName,RepositoryIssueRequest issueRequest)
   {
        var issues = await GetIssuesForRepository.GetIssuesForRepo(ownerName, repositoryName, issueRequest);
        this.issues.Value = issues.ToList();
   }
}
