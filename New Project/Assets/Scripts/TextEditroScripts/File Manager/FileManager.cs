using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FileManager : MonoBehaviour
{
    [SerializeField] private GameObject fileManagerContent;
    [SerializeField] private GameObject InputNameBackgorund;
    [SerializeField] private TMP_InputField nameOf;
    [SerializeField] private TextEditorManager textEditorManager;
    private TextEditorData textEditorData;
    private GenerateForDirectory generateForDirectory;
    private static string fileName;
    private static string directoryName;

    // Start is called before the first frame update
    void Start()
    {
        generateForDirectory =gameObject.GetComponent<GenerateForDirectory>();
        textEditorData = textEditorManager.textEditorData;
        directroyPrefab.OnFileCreationRequested += OnFileCreationRequestedHandler;
        directroyPrefab.OnDirectoryCreationRequested += OnDirectoryCreationRequestedHandler;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            InputNameBackgorund.SetActive(false);
        }
    }

    private void OnDirectoryCreationRequestedHandler(object sender, EventArgs e)
    {
        StartWaitingForInput((input) =>
        {
            directoryName = input;
            CreateDirectory();
        });
    }
    private void OnFileCreationRequestedHandler(object sender, EventArgs e)
    {
        StartWaitingForInput(input => {
            fileName = input;
            CreateFile();
            generateForDirectory.GenerateForDirectoy(fileManagerContent.transform, textEditorData.StartingDirecotry.ToString(), textEditorData);
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
    //File
    public void CreateFile()
    {
        string fullPath = Path.Combine(textEditorManager.textEditorData.WorkingDirectory.Value, fileName);
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
    //Direcotry
    public void CreateDirectory()
    {
        string fullPath = Path.Combine(textEditorManager.textEditorData.WorkingDirectory.ToString(), directoryName);
        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
            Debug.Log("Directory created: " + fullPath);
        }
        else
        {
            Debug.Log("Directory already exists: " + fullPath);
        }
    }

    public void DeleteDirectory(string directoryPath)
    {
        if (Directory.Exists(directoryPath))
        {
            Directory.Delete(directoryPath, true); // The second parameter is to delete recursively
            Debug.Log("Directory deleted: " + directoryPath);
        }
        else
        {
            Debug.Log("Directory does not exist.");
        }
    }

    public void RenameDirectory(string currentDirectoryPath, string newDirectoryPath)
    {
        if (Directory.Exists(currentDirectoryPath))
        {
            Directory.Move(currentDirectoryPath, newDirectoryPath);
            Debug.Log("Directory renamed from " + currentDirectoryPath + " to " + newDirectoryPath);
        }
        else
        {
            Debug.Log("Directory does not exist.");
        }
    }
}
