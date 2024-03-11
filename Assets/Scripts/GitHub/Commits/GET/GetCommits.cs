using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Octokit;

public class GetCommits : MonoBehaviour
{
    [SerializeField] private ApiRequestHelper apiRequestHelper;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task<Commit> GetAllComitsForRepo(string repo, string owner)
    {
        return new Commit();
    }

    public async Task<GitHubCommit> GetLastCommitForFile(string repo, string owner, string filePath)
    {
        var client = new GitHubClient(new ProductHeaderValue("YourAppName"));
        var commits = await client.Repository.Commit.GetAll(owner, repo, new CommitRequest { Path = filePath });
        
        if (commits.Count > 0)
        {
            GitHubCommit lastCommit = commits[0];
            return lastCommit;
        }
        else
        {
            throw new Exception("There was an error");
        }
    }  

    [Serializable]
    public class Commit
    {
        public string name;
    }
}
