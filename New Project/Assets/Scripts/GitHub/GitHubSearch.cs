using Octokit;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GitHubSearch : MonoBehaviour
{
    public static async Task<IReadOnlyList<Repository>> SearchRepositories(SearchRepositoriesRequest request)
    {
        var result = await GitHubClientProvider.client.Search.SearchRepo(request);
        return result.Items;
    }
}
