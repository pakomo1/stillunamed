using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Octokit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEditor.Progress;

public class GetUserRepoInfo : MonoBehaviour
{  
    [SerializeField] private RepoButtonTemplate buttonTemplate;
    public string json;

    public void GenerateButtons()
    {
        var repoInfo = JsonConvert.DeserializeObject<List<RepoInfo>>(json);
        for (int i = 0; i < repoInfo.Count; i++)
        {
            buttonTemplate.CreateButton(repoInfo[i].Name, repoInfo[i].Description, repoInfo[i].Owner.avatar_url, repoInfo[i].Private);
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
    }
}
