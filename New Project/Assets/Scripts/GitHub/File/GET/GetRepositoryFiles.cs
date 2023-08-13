using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Octokit;

public class GetRepositoryFiles : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private ApiRequestHelper apiRequestHelper;
    void Start()
    {
        
    }

    public async Task<IReadOnlyCollection<RepositoryContent>> GetRepoFiles(string repo, string owner,string path)
    {
        var contents = await GitHubClientProvider.client.Repository.Content.GetAllContents(owner, repo);
        return contents;
    }

    [Serializable]
    public class RepoContent
    {
        public string owner;
        public string repo;
        public string name;
        public string url;
        public string type;
        public string path;
    }
}
