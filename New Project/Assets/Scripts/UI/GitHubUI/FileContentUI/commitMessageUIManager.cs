using Octokit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class commitMessageUIManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField fileContentInputField;
    [SerializeField] private TMP_InputField fileName;
    [SerializeField] private TMP_InputField commitMessage;
    [SerializeField] private Button commitBtn;
    [SerializeField] private Button closeBtn;
    public event EventHandler OnCommit;

    private string path;
    private Repository currentRepositroy;
    // Start is called before the first frame update
    void Start()
    {
        commitBtn.onClick.AddListener(() =>
        {
            CreateFileRequest fileRequest = new CreateFileRequest(commitMessage.text, fileContentInputField.text);
            CommitFileToRepo(currentRepositroy, fileRequest);
        });

        closeBtn.onClick.AddListener(() =>
        {
            Hide();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Show(Repository repository, string path) {
        //if the path is equal to the repository name that means that we are in the root directroy
        this.path = (path == repository.Name) ? "" : path + "/";
       
        currentRepositroy = repository;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    private async void CommitFileToRepo(Repository repository, CreateFileRequest fileRequest)
    {
        path = $"{path}{fileName.text}";
        await AddFile.CreateFile(repository.Owner.Login, repository.Name, path , fileRequest);
        OnCommit.Invoke(this, EventArgs.Empty);
    }
}
