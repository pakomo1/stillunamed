using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RepoContentTemplate : MonoBehaviour
{
    [SerializeField] private GameObject fileTemplate;


    public void GenerateRepoFiles(List<GetRepositoryFiles.RepoContent> repoFiles)
    {
        foreach (var item in repoFiles)
        {
            var button = Instantiate(fileTemplate, transform);
            TextMeshProUGUI filename = button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            
            button.name = item.name;
            filename.text = item.name;
            button.SetActive(true);
        }
    }
}
