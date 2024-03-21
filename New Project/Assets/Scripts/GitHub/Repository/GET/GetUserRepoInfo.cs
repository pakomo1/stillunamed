using Cysharp.Threading.Tasks.CompilerServices;
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

    public async Task<bool> GenerateButtonsAsync()
    {
        for (int i = 0; i < repositories.Count; i++)
        {
            await buttonTemplate.CreateButtonAsync(repositories[i]);
        }
        return true;
    }
}
