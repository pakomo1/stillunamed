using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Octokit;
using System;
public class searchPublicRepoUiManager : MonoBehaviour
{
    [SerializeField]private TMP_InputField searchInputField;
     private SearchRepositoriesRequest request;

    void Start()
    {
        searchInputField.onValueChanged.AddListener(OnSearchInputFieldChanged);
        searchInputField.onSubmit.AddListener(OnSearchInputFieldSubmitted);
    }

    public async void OnSearchInputFieldSubmitted(string arg0)
    {
       var resutl = await GitHubSearch.SearchRepositories(request);
       foreach (var item in resutl)
       {
            Debug.Log(item.Name);
       }
    }

    private void OnSearchInputFieldChanged(string searchInput)
    {
        request = new SearchRepositoriesRequest(searchInput);
    }   
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
