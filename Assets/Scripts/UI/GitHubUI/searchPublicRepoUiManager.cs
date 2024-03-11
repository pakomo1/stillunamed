using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Octokit;
using System;
public class searchPublicRepoUiManager : MonoBehaviour
{
    [SerializeField]private TMP_InputField searchInputField;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField]private SearchRepoButtonTemplate searchRepoButtonTemplate;
     private SearchRepositoriesRequest request;

    [SerializeField] private GameObject loadingUi;
    [SerializeField] private GameObject searchContent;
    [SerializeField] private SingleActiveChild singleActiveChild;

    public event EventHandler OnStartGeneratingRepoButtons;
    void Start()
    {
        searchInputField.onValueChanged.AddListener(OnSearchInputFieldChanged);
        searchInputField.onSubmit.AddListener(OnSearchInputFieldSubmitted);

        OnStartGeneratingRepoButtons += OnStartGeneratingRepoButtonsHandler;
        searchRepoButtonTemplate.OnFinishedGeneratingRepoButtons += OnFinishedGeneratingRepoButtons;
    }

    public async void OnSearchInputFieldSubmitted(string arg0)
    {
       Hide();
        try
        {
            print("we here");
            OnStartGeneratingRepoButtons?.Invoke(this, EventArgs.Empty);
            var result = await GitHubSearch.SearchRepositories(request);
            searchRepoButtonTemplate.CreateButton(result);

        }catch(Exception ex)
        {
            print(ex.Message);
        }
       
    }

    private void OnFinishedGeneratingRepoButtons(object sender, EventArgs e)
    {
        print("Finished generating repo buttons");
        searchContent.SetActive(true);
        loadingUi.SetActive(false);
    }

    private void OnStartGeneratingRepoButtonsHandler(object sender, EventArgs e)
    {
        print("Started generating repo buttons");
        singleActiveChild.ActivateChild(3);
        loadingUi.SetActive(true);
        searchContent.SetActive(false);
    }


    private void OnSearchInputFieldChanged(string searchInput)
    {
        request = new SearchRepositoriesRequest(searchInput)
        {
           PerPage =11
        };
    }   
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Show()
    {
        gameObject.SetActive(true);
        rectTransform.anchoredPosition = Vector2.zero;
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
