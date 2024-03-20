using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class directroyPrefab : MonoBehaviour
{
    public GameObject createFile;
    public GameObject createDirecotry;


    private void Start()
    {
       createFile.GetComponent<Button>().onClick.AddListener(TextEditorManager.CreateFile);
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
