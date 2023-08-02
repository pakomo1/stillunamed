using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RepoFilesTemplate : MonoBehaviour
{
    [SerializeField] private GameObject fileTemplate;
    [SerializeField] private Sprite file;
    [SerializeField] private Sprite dir;

    public void GenerateRepoFiles(List<GetRepositoryFiles.RepoContent> repoFiles)
    {
        ClearFiled();
        foreach (var item in repoFiles)
        {
            var button = Instantiate(fileTemplate, transform);
            TextMeshProUGUI filename = button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            button.name = item.name;
            filename.text = item.name;

            Image image = button.transform.GetChild(1).GetComponent<Image>();
            if(item.type == "dir")
            {
                image.sprite = dir;
            }else if(item.type == "file")
            {
                image.sprite = file;    
            }
            button.GetComponent<Button>().onClick.AddListener(() => ) 
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
