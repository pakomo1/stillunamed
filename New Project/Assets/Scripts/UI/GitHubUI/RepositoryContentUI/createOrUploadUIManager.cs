using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Octokit;
using TMPro;

public class createOrUploadUIManager : MonoBehaviour
{
    [SerializeField] private Button createFileButton;
    [SerializeField] private Button uploadFileButton;

    [SerializeField] private FileContentUIManager fileContentUI;
    [SerializeField] private SingleActiveChild singleActiveChild;

    [SerializeField] private fileMakeUpManager fileMakeUpManager;
    [SerializeField] private RepoContentNavigation repositoryContentNavigation;

    [SerializeField] private TextMeshProUGUI pathToFile;

    void Start()
    {
        createFileButton.onClick.AddListener(() =>
        {
            
            singleActiveChild.ActivateChild(2);
            fileMakeUpManager.Show(repositoryContentNavigation.currentRepository, pathToFile.text);
        });

        uploadFileButton.onClick.AddListener(() =>
        {

        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
