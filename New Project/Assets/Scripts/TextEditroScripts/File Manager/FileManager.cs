using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    [SerializeField] private GameObject fileManagerContent;
    private GenerateForDirectory generateForDirectory;
    // Start is called before the first frame update
    void Start()
    {
        generateForDirectory =gameObject.GetComponent<GenerateForDirectory>();
       // generateForDirectory.GenerateForDirectoy(fileManagerContent.transform, @"C:\Users\Maixm\Desktop\DZI");
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CreateFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            File.Create(filePath);
        }
        else
        {
            Debug.Log("File already exists.");
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
