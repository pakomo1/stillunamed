using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Octokit;

public class RepoFilesTemplate : MonoBehaviour
{
    [SerializeField] private GameObject fileTemplate;
    [SerializeField] private Sprite file;
    [SerializeField] private Sprite dir;
    [SerializeField] private FilesContentNavigation filesContentNavigation;
    [SerializeField] private GameObject repositoryContentUI;
    [SerializeField] private GameObject FileContentCode;

    public void GenerateRepoFiles(IReadOnlyCollection<RepositoryContent> repoFiles, Repository repository, string currentbranch, List<string> breadcrumb)
    {
        ClearFiled();
        repoFiles = repoFiles.OrderByDescending(item => item.Type == "dir").ToList();
        // Add a ".." button to navigate up
        if (breadcrumb.Count > 0)
        {
            var button = Instantiate(fileTemplate, transform);
            TextMeshProUGUI filename = button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            button.name = "..";
            filename.text = "..";
            button.GetComponent<Button>().onClick.AddListener(() => { filesContentNavigation.NavigateUp(repository, currentbranch); });
            button.SetActive(true);
        }

        foreach (var item in repoFiles)
        {
            var button = Instantiate(fileTemplate, transform);
            TextMeshProUGUI filename = button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
             button.name = item.Name;
             filename.text = item.Name;

            Image image = button.transform.GetChild(1).GetComponent<Image>();
            if(item.Type == "dir")
            {
                image.sprite = dir;
            }
            else if(item.Type == "file")
            {
                button.GetComponent<Button>().onClick.AddListener(() => 
                {
                    ActivateObjectInContent.OnClickSwitchToThisUI(repositoryContentUI, FileContentCode);
                });

                image.sprite = file;    
            }
            button.GetComponent<Button>().onClick.AddListener(() => { filesContentNavigation.ShowFileContent(item, repository, currentbranch); });
            button.SetActive(true);
        }
    }
    private void ClearFiled()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        
    }
   
}
