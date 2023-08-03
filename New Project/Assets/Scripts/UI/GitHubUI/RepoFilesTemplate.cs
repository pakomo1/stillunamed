using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RepoFilesTemplate : MonoBehaviour
{
    [SerializeField] private GameObject fileTemplate;
    [SerializeField] private Sprite file;
    [SerializeField] private Sprite dir;
    [SerializeField] private FilesContentNavigation filesContentNavigation;
    [SerializeField] private ApiRequestHelper apiRequestHelper;
    [SerializeField] private GameObject repositoryContentUI;
    [SerializeField] private GameObject FileContentPanel;

    public void GenerateRepoFiles(List<GetRepositoryFiles.RepoContent> repoFiles)
    {
        ClearFiled();

        repoFiles = repoFiles.OrderByDescending(item => item.type == "dir").ToList();
        foreach (var item in repoFiles)
        {
            print(item.url);
           
            var button = Instantiate(fileTemplate, transform);
            TextMeshProUGUI filename = button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
             button.name = item.name;
             filename.text = item.name;

            Image image = button.transform.GetChild(1).GetComponent<Image>();
            if(item.type == "dir")
            {
                image.sprite = dir;
            }
            else if(item.type == "file")
            {
                button.GetComponent<Button>().onClick.AddListener(() => 
                {
                    ActivateObjectInContent.OnClickSwitchToThisUI(repositoryContentUI, FileContentPanel);
                });

                image.sprite = file;    
            }
            button.GetComponent<Button>().onClick.AddListener(() => { filesContentNavigation.ShowFileContent(item); });
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
