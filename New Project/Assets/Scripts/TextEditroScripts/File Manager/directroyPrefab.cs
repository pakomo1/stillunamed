using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class directroyPrefab : MonoBehaviour
{
    public GameObject createFile;
    public GameObject createDirecotry;

    public static event EventHandler OnFileCreationRequested;
    public static event EventHandler OnDirectoryCreationRequested;

    private void Start()
    {
       createFile.GetComponent<Button>().onClick.AddListener(()=> 
       {
           OnFileCreationRequested?.Invoke(this, EventArgs.Empty);
       });
        createDirecotry.GetComponent<Button>().onClick.AddListener(()=>
        {
              OnDirectoryCreationRequested?.Invoke(this, EventArgs.Empty);
        });
    }

    public void OnPointerEnter()
    {
        // Show the buttons
        createFile.SetActive(true);
        createDirecotry.SetActive(true);
    }
    public void OnPointerExit()
    {
        // Hide the buttons
        createFile.SetActive(false);
        createDirecotry.SetActive(false);
    }
}
