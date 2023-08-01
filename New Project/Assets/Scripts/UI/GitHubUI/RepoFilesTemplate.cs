using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RepoFilesTemplate : MonoBehaviour
{
    [SerializeField] private GameObject fileTemplate;


    public void GenerateRepoFiles(List<GetRepositoryFiles.RepoContent> repoFiles)
    {
        ClearFiled();
        foreach (var item in repoFiles)
        {
            var button = Instantiate(fileTemplate, transform);
            TextMeshProUGUI filename = button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            
            button.name = item.name;
            filename.text = item.name;
            button.SetActive(true);
        }
    }
    private void ClearFiled()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == 0)
                continue;
            Destroy(transform.GetChild(i).gameObject);
        }
        
    }
}
