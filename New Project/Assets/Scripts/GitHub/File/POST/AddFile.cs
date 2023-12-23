using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Octokit;

public class AddFile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public static async Task<RepositoryContentChangeSet> CreateFile(string owner, string repo, string path, CreateFileRequest fileRequest)
    {
        RepositoryContentChangeSet contents = await GitHubClientProvider.client.Repository.Content.CreateFile(owner, repo,path, fileRequest);
        return contents;
    }
}
