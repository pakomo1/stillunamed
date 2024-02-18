using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class publicRepoDisplayUIManager : MonoBehaviour
{
    [SerializeField] private SearchRepoButtonTemplate searchRepoButtonTemplate;
    [SerializeField] private searchPublicRepoUiManager searchPublicRepoUiManager;
    [SerializeField] private GameObject loadingUi;
    [SerializeField] private GameObject searchContent;
    // Start is called before the first frame update
    void Start()
    {
        searchPublicRepoUiManager.OnStartGeneratingRepoButtons += OnStartGeneratingRepoButtons;
        searchRepoButtonTemplate.OnFinishedGeneratingRepoButtons += OnFinishedGeneratingRepoButtons;
    }

    private void OnFinishedGeneratingRepoButtons(object sender, EventArgs e)
    {
        print("Finished generating repo buttons"); 
        searchContent.SetActive(true);  
        loadingUi.SetActive(false);
    }

    private void OnStartGeneratingRepoButtons(object sender, EventArgs e)
    {
        print("Started generating repo buttons");
        loadingUi.SetActive(true);
        searchContent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
