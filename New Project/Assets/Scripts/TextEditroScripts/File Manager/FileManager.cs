using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    [SerializeField] private GameObject fileManagerContent;
    [SerializeField] private GameObject InputNameBackgorund;
    [SerializeField] private TMP_InputField nameOf;
     private static TextEditorManager textEditorManager;
     private GenerateForDirectory generateForDirectory;
     private static string fileName;

    // Start is called before the first frame update
    void Start()
    {
        generateForDirectory =gameObject.GetComponent<GenerateForDirectory>();
        textEditorManager = gameObject.GetComponentInParent<TextEditorManager>();
        directroyPrefab.OnFileCreationRequested += OnFileCreationRequestedHandler;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            InputNameBackgorund.SetActive(false);
        }
    }

    private void OnFileCreationRequestedHandler(object sender, EventArgs e)
    {
        StartWaitingForInput(input => {
            fileName = input;
            CreateFile();
        });
    }

    private void StartWaitingForInput(Action<string> callback)
    {
        StartCoroutine(WaitForInput(callback));
    }

    private IEnumerator WaitForInput(Action<string> callback)
    {
        // Enable the input field
        InputNameBackgorund.SetActive(true);
        nameOf.Select();
        nameOf.ActivateInputField();

        // Wait until the user has submitted the input field
        while (!Input.GetKeyDown(KeyCode.Return) || string.IsNullOrEmpty(nameOf.text))
        {
            yield return null;
        }

        // The user has submitted the input field, so you can continue execution here
        string input = nameOf.text;
        InputNameBackgorund.SetActive(false);

        // Call the callback function
        callback(input);
    }

    public void CreateFile()
    {
        string fullPath = Path.Combine(textEditorManager.textEditorData.WorkingDirectory.ToString(), fileName);
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
    public void DeleteFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        else
        {
            Debug.Log("File does not exist.");
        }
    }

    public void RenameFile(string currentFilePath, string newFilePath)
    {
        if (File.Exists(currentFilePath))
        {
            File.Move(currentFilePath, newFilePath);
        }
        else
        {
            Debug.Log("File does not exist.");
        }
    }
}
