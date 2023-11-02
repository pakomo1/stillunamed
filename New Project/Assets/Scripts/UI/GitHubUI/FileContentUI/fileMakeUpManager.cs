using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Octokit;
using TMPro;

public class fileMakeUpManager : MonoBehaviour
{

    [SerializeField] private Button commitButton;
    [SerializeField] private commitMessageUIManager enterCommitMessageUI;
    [SerializeField] private TextMeshProUGUI pathToFile;
    [SerializeField] private TMP_InputField fileName;

    private Repository currentRepository;

    // Start is called before the first frame update
    void Start()
    {
        commitButton.onClick.AddListener(() =>
        {
            if(fileName.text != "")
            {
                enterCommitMessageUI.Show(currentRepository, pathToFile.text);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Show(Repository currentRepo, string path)
    {
        pathToFile.text = path;
        currentRepository = currentRepo;
        gameObject.SetActive(true);
    }
    public void Hide() { this.gameObject.SetActive(false); }
 

}
