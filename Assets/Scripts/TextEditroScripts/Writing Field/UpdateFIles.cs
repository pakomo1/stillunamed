using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class UpdateFIles : MonoBehaviour
{
    private ReadFiles readFiles;
    [SerializeField] private GameObject TextFieldManager;

    //these two come form the ReadFiles class
    private string selectedFilePath;
    private TMP_InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
        readFiles = TextFieldManager.GetComponent<ReadFiles>();

        this.selectedFilePath = readFiles.selectedFilePath;
        this.inputField = readFiles.inputfield;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
          if(CheckIfDifferent(inputField.text, File.ReadAllText(selectedFilePath)))
           {
            print("The file has been saved");
                 File.WriteAllText(selectedFilePath, inputField.text);
            }
        }
    }

    public bool CheckIfDifferent(string text1, string text2)
    {
        if(text1 != text2)
        {
            return true;
        }

        return false;
    }
}
