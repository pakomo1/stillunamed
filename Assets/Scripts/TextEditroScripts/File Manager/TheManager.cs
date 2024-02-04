using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;
using UnityEditor.UIElements;

public class TheManager : MonoBehaviour
{
    [SerializeField] private GameObject fileManager;
    [SerializeField] private GameObject TextFieldManager;
    [SerializeField] private TextEditor textEditor;

    [SerializeField] private GameObject filePrefab;
    [SerializeField] private GameObject directoryPrefab;
    [SerializeField] private GameObject dirContentHolder;
    [SerializeField] private Transform rootPanel;
    private OpenFolder openfolder;
    // Start is called before the first frame update
    void Start()
    {
        openfolder = TextFieldManager.GetComponent<OpenFolder>();
    }

    // Update is called once per frame
    public void GenerateForDirectoy(Transform parentPanel,string directory)
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
                directoryEntry.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = Path.GetFileName(item);
                directoryEntry.tag = "directory";

                var layoutEl = directoryEntry.AddComponent<LayoutElement>();
                layoutEl.preferredHeight = 30;
                
                var direcotryContentHolder = Instantiate(dirContentHolder, parentPanel);
                direcotryContentHolder.name = "dirContentHolder"; 
                
                directoryEntryButton.onClick.AddListener(() => ExpandDirecotryContent(direcotryContentHolder.transform));
                GenerateForDirectoy(direcotryContentHolder.transform, item);
            }
            else if(File.Exists(item))
            {
                var fileEntry = Instantiate(filePrefab, parentPanel);
                fileEntry.name = Path.GetFileName(item);

                fileEntry.tag = "file";
                fileEntry.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = Path.GetFileName(item);

                var fileEntryButton = fileEntry.GetComponent<Button>();
                fileEntryButton.onClick.AddListener(() => textEditor.PathToTheSelectedFile = item);
            }
        }

    }
    private void ExpandDirecotryContent(Transform panel)
    {
        panel.gameObject.SetActive(!panel.gameObject.activeSelf);
    }
    private void ChangeButtonColorWhenPressed(Button button)
    {
        ColorBlock colorPallete = button.colors;
        Color lighterColor = colorPallete.normalColor;

        lighterColor.r = Mathf.Min(lighterColor.r + 0.2f, 1);
        lighterColor.g = Mathf.Min(lighterColor.g + 0.2f, 1);
        lighterColor.b = Mathf.Min(lighterColor.b + 0.2f, 1);

        colorPallete.normalColor = lighterColor;
        button.colors = colorPallete;
    }

}
