using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Octokit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class GetUserRepoInfo : MonoBehaviour
{  
    [SerializeField] private RepoButtonTemplate buttonTemplate;
    public IReadOnlyList<Repository> repositories;

    public void GenerateButtons()
    {
        for (int i = 0; i < repositories.Count; i++)
        {
            buttonTemplate.CreateButton(repositories[i].Name, repositories[i].Description, repositories[i].Owner.AvatarUrl, repositories[i].Private, repositories[i].Owner.Login);
        }
    }
    public class RepoInfo 
    {
        public string Name;
        public string Description;
        public bool Private;
        public OwnerInfo Owner;
    }
    public class OwnerInfo 
    {
        public string avatar_url;
        public string login;
    }
}
