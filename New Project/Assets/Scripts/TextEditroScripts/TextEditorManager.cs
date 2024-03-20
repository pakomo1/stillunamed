using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using TMPro;
using System.IO;
using System;
using System.Threading.Tasks;
using UnityEngine.EventSystems;
using Unity.Collections;
using UnityEngine.InputSystem.LowLevel;
using Unity.VisualScripting;

public class TextEditorManager : MonoBehaviour
{
    private InGameTextEditor.TextEditor textEditor;
    [SerializeField] private GenerateForDirectory directoryManager;
    [SerializeField] private GameObject fileManagerContent;
    public static TextEditorData textEditorData;
    private string previousText;

    private void Start()
    {
        textEditorData.OnSelectedFileChanged += OnFileSelectedHandlerAsync;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }

        // Check if the text has changed
        if (textEditor != null && textEditor.Text != previousText)
        {
            // Update the DisplayText of TextEditorData
            textEditorData.DisplayText = textEditor.Text;

            // Save the current text for the next comparison
            previousText = textEditor.Text;
        }
    }

    private async void OnFileSelectedHandlerAsync(FixedString128Bytes filePath)
    {
         string fileContent = await File.ReadAllTextAsync(filePath.ToString());
         print(fileContent);
         textEditor.SetText(fileContent);
    }
    public void LoadEditorData(TextEditorData data)
    {
        textEditor = transform.GetChild(1).transform.GetChild(0).GetComponent<InGameTextEditor.TextEditor>();
        gameObject.SetActive(true);

        textEditorData = data;
        string text = data.DisplayText.ToString();
        textEditor.CaretPosition = new InGameTextEditor.TextPosition(0, 0);

        textEditor.SetText(text);
        directoryManager.GenerateForDirectoy(fileManagerContent.transform,textEditorData.WorkingDirectory.Value, textEditorData);
    }
    public static void CreateFile()
    {
        string fileName = "NewFile.txt";
        string fullPath = Path.Combine(textEditorData.WorkingDirectory.ToString(), fileName);
        if (!File.Exists(fullPath))
        {
            using (File.Create(fullPath))
            {
                Debug.Log("File created: " + fullPath);
            }
        }
        else
        {
            Debug.Log("File already exists: " + fullPath);
        }
    }
}
