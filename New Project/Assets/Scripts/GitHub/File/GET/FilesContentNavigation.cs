using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using Octokit;

public class FilesContentNavigation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI contentDisplayField;
    [SerializeField] private ApiRequestHelper apiRequestHelper;
    [SerializeField] private RepoFilesTemplate repoFilesTemplate;
    [SerializeField] private TextMeshProUGUI currentFileTextField;
    [SerializeField] private TextMeshProUGUI dirPathNavigationText;
    [SerializeField] private TextMeshProUGUI filePathNavigationText;

    [SerializeField] private GetRepositoryFiles getRepositoryFiles;
    public async void ShowFileContent( RepositoryContent fileOrDir, Repository repository, string currentbranch)
    {
        var client = GitHubClientProvider.client;
        try
        {
            if (fileOrDir.Type == "dir")
            {
                var content = await GetRepositoryFiles.GetRepoFiles(repository.Owner.Login, repository.Name, fileOrDir.Path, currentbranch);

                dirPathNavigationText.text = $"{repository.Name}/{fileOrDir.Path}";
                repoFilesTemplate.GenerateRepoFiles(content, repository, currentbranch);

            }
            else if (fileOrDir.Type == "file")
            {
                string fileName = Path.GetFileName(fileOrDir.Path);
                //  string result = System.Text.Encoding.UTF8.GetString();

                filePathNavigationText.text = $"{repository.Name}/{fileName}";
                contentDisplayField.text = fileOrDir.Content;
                currentFileTextField.text = fileOrDir.Name;
            }
        }catch(Exception ex)
        {
            print(ex.Message);
        }
       
        
    }

}
