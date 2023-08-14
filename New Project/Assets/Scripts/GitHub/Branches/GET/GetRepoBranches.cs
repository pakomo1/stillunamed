using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Octokit;
using System.Threading.Tasks;

public class GetRepoBranches : MonoBehaviour
{
    public static async Task<IReadOnlyCollection<Branch>> GetBranches(string owner, string repoName)
    {
       IReadOnlyCollection<Branch> repoBranches = await GitHubClientProvider.client.Repository.Branch.GetAll(owner, repoName);
       return repoBranches;
    }
}
