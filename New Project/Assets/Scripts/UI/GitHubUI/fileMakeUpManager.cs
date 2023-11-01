using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Octokit;

public class fileMakeUpManager : MonoBehaviour
{

    [SerializeField] private GameObject fileInputField;
    [SerializeField] private Button commitButton;
    [SerializeField] private GameObject areYouSureUI;
    [SerializeField] private GameObject pathToFile;

    private Repository currentRepository;

    // Start is called before the first frame update
    void Start()
    {
        commitButton.onClick.AddListener(() =>
        {
            areYouSureUI.SetActive(true);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Show(Repository currentRepo)
    {
        currentRepository = currentRepo;
        gameObject.SetActive(true);
    }
    public void Hide() { this.gameObject.SetActive(false); }

}
