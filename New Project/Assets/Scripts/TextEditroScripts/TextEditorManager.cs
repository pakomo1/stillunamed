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
    }

    private async void OnFileSelectedHandlerAsync(FixedString128Bytes filePath)
    {
         string fileContent = await File.ReadAllTextAsync(filePath.ToString());
         print(fileContent);
         textEditor.SetText(fileContent);
    }
}
