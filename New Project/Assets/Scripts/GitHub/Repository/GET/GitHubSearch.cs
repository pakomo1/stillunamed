using Octokit;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GitHubSearch : MonoBehaviour
{
    public static async Task SearchRepositories(SearchRepositoriesRequest request)
    {
        var result = await GitHubClientProvider.client.Search.SearchRepo(request);

        foreach (var repo in result.Items)
        {
            Debug.Log(repo.FullName);
        }
    }
}
