using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using TMPro;
using System.IO;
using System;
using System.Threading.Tasks;
using UnityEngine.EventSystems;

public class TextEditorManager : MonoBehaviour
{
    private InGameTextEditor.TextEditor textEditor;
    private TextEditorData textEditorData;

    private void Start()
    {
        textEditor = transform.GetChild(0).GetComponent<InGameTextEditor.TextEditor>();
        textEditorData = gameObject.GetComponentInParent<TextEditorData>();
        textEditorData.OnSelectedFileChanged += OnFileSelectedHandlerAsync;
    }
    private void Update()
    {
        print(textEditorData.Id);
    }

    private async void OnFileSelectedHandlerAsync(string filePath)
    {
         string fileContent = await File.ReadAllTextAsync(filePath);
         print(fileContent);
         textEditor.SetText(fileContent);
    }
}
