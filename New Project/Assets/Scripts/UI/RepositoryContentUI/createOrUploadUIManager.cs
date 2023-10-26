using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class createOrUploadUIManager : MonoBehaviour
{
    [SerializeField] private Button createFileButton;
    [SerializeField] private Button uploadFileButton;

    [SerializeField] private GameObject repositoryContentUI;
    [SerializeField] private GameObject fileContentUI;
    // Start is called before the first frame update
    void Start()
    {
        createFileButton.onClick.AddListener(() =>
        {
            repositoryContentUI.SetActive(false);

            fileContentUI.SetActive(true);
            fileContentUI.transform.Find("fileMakeUp").gameObject.SetActive(true);
        });

        uploadFileButton.onClick.AddListener(() =>
        {

        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
