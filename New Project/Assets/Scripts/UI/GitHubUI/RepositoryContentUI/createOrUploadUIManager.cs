using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Octokit;

public class createOrUploadUIManager : MonoBehaviour
{
    [SerializeField] private Button createFileButton;
    [SerializeField] private Button uploadFileButton;

    [SerializeField] private GameObject repositoryContentUI;
    [SerializeField] private GameObject fileContentUI;

    [SerializeField] private fileMakeUpManager fileMakeUpManager;
    [SerializeField] private RepositoryContentNavigation repositoryContentNavigation;

    void Start()
    {
        createFileButton.onClick.AddListener(() =>
        {
            repositoryContentUI.SetActive(false);

            fileContentUI.SetActive(true);
            fileMakeUpManager.Show(repositoryContentNavigation.currentRepository);
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
