using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Octokit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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
}
