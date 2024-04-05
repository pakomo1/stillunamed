using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Octokit;
using TMPro;
using System;
public class branchesTemplate : MonoBehaviour
{
    [SerializeField] private Button branchButtonTemplate;
    [SerializeField] private RepoContentNavigation repositoryContentNavigation;
    public event Action<string> OnBranchSelected;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateBranchs(IReadOnlyCollection<Branch> repositoryBranches, Repository currentRepostiory)
    {
        ClearChildern();
        foreach (Branch branch in repositoryBranches)
        {
            var button = Instantiate(branchButtonTemplate, transform);
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = branch.Name;
            button.name = branch.Name;

            button.onClick.AddListener(() =>
            {
                repositoryContentNavigation.ShowRepositoryContent(currentRepostiory.Owner.Login,currentRepostiory.Name, ".", branch.Name);  
                OnBranchSelected?.Invoke(branch.Name);
            });
            button.gameObject.SetActive(true);
        }
    }

    private void ClearChildern()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
