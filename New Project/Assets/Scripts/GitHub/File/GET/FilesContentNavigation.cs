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
    [SerializeField] private RepoFilesTemplate repoFilesTemplate;
    public List<string> Breadcrumb { get; private set; } = new List<string>(); // The breadcrumb list is now public

    public async void ShowFileContent(RepositoryContent fileOrDir, Repository repository, string currentbranch)
    {
        try
        {
            if (fileOrDir.Type == "dir")
            {
                Breadcrumb.Add(fileOrDir.Path); // Add the directory to the breadcrumb

                var content = await GetRepositoryFiles.GetRepoFiles(repository.Owner.Login, repository.Name, fileOrDir.Path, currentbranch);

                repoFilesTemplate.GenerateRepoFiles(content, repository, currentbranch, Breadcrumb);

            }
            else if (fileOrDir.Type == "file")
            {
                string fileName = Path.GetFileName(fileOrDir.Path);
                print(fileName);
            }
        }
        catch (Exception ex)
        {
            print(ex.Message);
        }
    }

    public async void NavigateUp(Repository repository, string currentbranch)
    {
        if (Breadcrumb.Count > 0)
        {
            Breadcrumb.RemoveAt(Breadcrumb.Count - 1); // Remove the last directory from the breadcrumb

            string path = Breadcrumb.Count > 0 ? Breadcrumb[Breadcrumb.Count - 1] : "\\"; // The path is null if the breadcrumb is empty

            var content = await GetRepositoryFiles.GetRepoFiles(repository.Owner.Login, repository.Name, path, currentbranch);

            repoFilesTemplate.GenerateRepoFiles(content, repository, currentbranch, Breadcrumb);
        }
    }
}

