using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class FilesContentNavigation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI contentDisplayField;
    [SerializeField] private ApiRequestHelper apiRequestHelper;
    [SerializeField] private RepoFilesTemplate repoFilesTemplate;
    [SerializeField] private TextMeshProUGUI currentFileTextField;
    [SerializeField] private TextMeshProUGUI dirPathNavigationText;
    [SerializeField] private TextMeshProUGUI filePathNavigationText;
    public async void ShowFileContent(GetRepositoryFiles.RepoContent fileOrDir)
    {
        print(fileOrDir.path);
        string fileContentDataRaw = await apiRequestHelper.GetRequestCreator(fileOrDir.url);

        
        if (fileOrDir.type == "dir")
        {
            dirPathNavigationText.text = $"{dirPathNavigationText.text}/{Path.GetFileName(fileOrDir.path)}";
            List<GetRepositoryFiles.RepoContent> files = JsonConvert.DeserializeObject<List<GetRepositoryFiles.RepoContent>>(fileContentDataRaw);
            repoFilesTemplate.GenerateRepoFiles(files);

        }else if(fileOrDir.type == "file")
        {
            string fileName = Path.GetFileName(fileOrDir.path);
            FileContent fileContent = JsonConvert.DeserializeObject<FileContent>(fileContentDataRaw);
            // Convert the Base64 string to a byte array
            byte[] byteArray = Convert.FromBase64String(fileContent.content);
            // Convert the byte array back to a string
            string result = System.Text.Encoding.UTF8.GetString(byteArray);

            filePathNavigationText.text = $"{dirPathNavigationText.text}/{fileName}";
            contentDisplayField.text = result;
            currentFileTextField.text = fileOrDir.name;
        }
        
    }
    [Serializable]
    public class FileContent
    {
        public string content;
    }

}
