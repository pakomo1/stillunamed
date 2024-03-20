using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class GenerateForDirectory : MonoBehaviour
{

    [SerializeField] private GameObject filePrefab;
    [SerializeField] private GameObject directoryPrefab;
    [SerializeField] private GameObject dirContentHolder;

    public static event EventHandler OnFileSelected;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    public void GenerateForDirectoy(Transform parentPanel, string directory,TextEditorData textEditorData, int depth = 0)
    {
        var directoriesInDir = Directory.GetDirectories(directory);
        var filesInDir = Directory.GetFiles(directory);

        List<string> allFilesAndDirectories = new List<string>();
        allFilesAndDirectories.AddRange(directoriesInDir);
        allFilesAndDirectories.AddRange(filesInDir);

        foreach (var item in allFilesAndDirectories)
        {
            if (Directory.Exists(item))
            {
                var directoryEntry = Instantiate(directoryPrefab, parentPanel);
                var directoryEntryButton = directoryEntry.GetComponent<Button>();

                directoryEntry.name = Path.GetFileName(item);
                var textComponent = directoryEntry.transform.GetChild(0).gameObject;
                textComponent.GetComponent<TextMeshProUGUI>().text = Path.GetFileName(item);

                // Adjust the position of the GameObject
                directoryEntry.transform.localPosition = new Vector3(depth * 10, directoryEntry.transform.localPosition.y, directoryEntry.transform.localPosition.z);

                //textComponent.margin = new Vector4(depth * 10, textComponent.margin.y, textComponent.margin.z, textComponent.margin.w); // Add left padding

                directoryEntry.tag = "directory";

                var direcotryContentHolder = Instantiate(dirContentHolder, parentPanel);
                direcotryContentHolder.name = $"{directoryEntry.name}ContentHolder";

                directoryEntryButton.onClick.AddListener(() => {
                    ExpandDirecotryContent(direcotryContentHolder.transform, item , textEditorData); 
                });
                GenerateForDirectoy(direcotryContentHolder.transform, item ,textEditorData, depth + 1); // Increase depth for nested directories
            }
            else if (File.Exists(item))
            {
                var fileEntry = Instantiate(filePrefab, parentPanel);
                fileEntry.name = Path.GetFileName(item);
                fileEntry.tag = "file";

                var textComponent = fileEntry.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
                textComponent.text = Path.GetFileName(item);
                // Add left padding
                textComponent.margin = new Vector4(depth * 10, textComponent.margin.y, textComponent.margin.z, textComponent.margin.w); // Add left padding

                var fileEntryButton = fileEntry.GetComponent<Button>();
                fileEntryButton.onClick.AddListener(() => { textEditorData.PathToTheSelectedFile = item; OnFileSelected?.Invoke(this, EventArgs.Empty); });
            }
        }
    }

    private void ExpandDirecotryContent(Transform panel, string directory, TextEditorData textEditorData)
    {
        bool isActive = panel.gameObject.activeSelf;

        if (isActive)
        {
            textEditorData.WorkingDirectory = Path.GetDirectoryName(textEditorData.WorkingDirectory.ToString());
        }
        else
        {
            textEditorData.WorkingDirectory = Path.Combine(textEditorData.WorkingDirectory.ToString(), Path.GetFileName(directory));
        }

        panel.gameObject.SetActive(!isActive);
    }

}
