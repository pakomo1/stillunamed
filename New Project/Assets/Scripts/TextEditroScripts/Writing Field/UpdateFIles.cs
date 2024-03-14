using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Diagnostics;
using System;
using System.Linq.Expressions;
using System.Collections;
using System.Threading.Tasks;

public class UpdateFIles : MonoBehaviour
{
    [SerializeField] private TextEditorData textEditor; 
    [SerializeField] private GameObject TextFieldManager;

    [SerializeField] private GameObject canvas;
   
   [SerializeField] private TMP_InputField inputField;
    private string selectedFilePath;

    public string currentSaveDate;
    // Start is called before the first frame update
    void Start()
    {
        selectedFilePath = textEditor.PathToTheSelectedFile;
        inputField.text = textEditor.DisplayText;

        inputField.onValueChanged.AddListener((value) => { textEditor.DisplayText = value;});
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (CheckIfDifferent(inputField.text, File.ReadAllText(selectedFilePath)))
            {
                File.WriteAllText(selectedFilePath, inputField.text);
                currentSaveDate = DateTime.Now.ToString();
            }
        }
    }
        public bool CheckIfDifferent(string text1, string text2)
        {
            if (text1 != text2)
            {
                return true;
            }

            return false;
        }
}
