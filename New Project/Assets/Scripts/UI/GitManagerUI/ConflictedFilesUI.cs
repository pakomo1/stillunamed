using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConflictedFilesUI : MonoBehaviour
{
    [SerializeField] private GameObject conflictedFilesContent;
    [SerializeField] private GameObject conflictedFileTemplate;
    [SerializeField] private GameObject gitManagerUI;
    [SerializeField] private TextEditorManager textEditorManager;
    public void DisplayConflictedFiles(List<string> conflictedFiles)
    {
        foreach (Transform child in conflictedFilesContent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var file in conflictedFiles)
        {
            var conflictedFile = Instantiate(conflictedFileTemplate, conflictedFilesContent.transform);
            var filePath = Path.Combine(textEditorManager.textEditorData.StartingDirecotry.ToString(), file);
            conflictedFile.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = filePath;
            conflictedFile.GetComponent<Button>().onClick.AddListener(() => { OpenConflictedFile(filePath); });

            conflictedFile.SetActive(true);
        }
    }
    private async void OpenConflictedFile(string filePath)
    {
        if (textEditorManager.textEditorData.PathToTheSelectedFile == filePath)
        {
            string fileContent = await File.ReadAllTextAsync(filePath.ToString());
            textEditorManager.WriteText(fileContent);
        }
        else
        {
            textEditorManager.textEditorData.PathToTheSelectedFile = filePath;
        }
        gitManagerUI.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
}
