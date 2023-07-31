using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class GetRepositoryContent : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private ApiRequestHelper apiRequestHelper;
    void Start()
    {
        
    }

    public async Task<List<RepoContent>> GetRepoContents(string repo, string owner, string path)
    {
        string url = $"https://api.github.com/repos/{owner}/{repo}/contents/{path}";
        string data = await apiRequestHelper.GetRequestCreator(url);

        List<RepoContent> contentList = JsonConvert.DeserializeObject<List<RepoContent>>(data);
        return contentList;
    }

    [Serializable]
    public class RepoContent
    {
        public string name;
    }
}
