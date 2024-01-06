using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Octokit;
using UnityEngine.UI;

public class GetRepositoryFiles : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private ApiRequestHelper apiRequestHelper;
    [SerializeField] private GameObject repoPanelContent;
    void Start()
    {
        
        repoPanelContent.transform.GetChild(0).GetComponent<Button>().onClick.Invoke();
    }

    public static async Task<IReadOnlyCollection<RepositoryContent>> GetRepoFiles( string owner, string repo, string path, string branch = "main")
    {
        var repository = await GitHubClientProvider.client.Repository.Get(owner, repo);

        var contents = await GitHubClientProvider.client.Repository.Content.GetAllContentsByRef(owner, repo, path, branch);
        return contents;
    }
}
