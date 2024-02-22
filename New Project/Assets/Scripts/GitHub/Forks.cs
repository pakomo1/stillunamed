using Octokit;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Forks : MonoBehaviour
{
    public static async Task<Repository> ForkRepository(string owner, string repoName)
    {
        var fork = await GitHubClientProvider.client.Repository.Forks.Create(owner, repoName, new NewRepositoryFork());
        return fork;
    }
}
