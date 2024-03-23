using UnityEngine;
using TMPro;
using System.IO;
using Unity.Collections;
using Unity.VisualScripting;
using System;
using System.Threading.Tasks;

public class TextEditorManager : MonoBehaviour
{
    [SerializeField]private InGameTextEditor.TextEditor textEditor;
    [SerializeField] private GenerateForDirectory directoryManager;
    [SerializeField] private GameObject fileManagerContent;
    [SerializeField] private GitManager gitManagerUi;
    [SerializeField] private GameObject textEditorHolder;
    public TextEditorData textEditorData;
    private string previousText;

    public static TextEditorManager Instance { get; private set; }
    private event EventHandler OnTextEditorLoaded;
    private bool isTextEditorLoaded = false;
    private void Start()
    {
        if (Instance == null)
        {
            Instance = gameObject.GetComponent<TextEditorManager>();
        }
        OnTextEditorLoaded += TextEditorManager_OnTextEditorLoaded;
    }

    private void TextEditorManager_OnTextEditorLoaded(object sender, EventArgs e)
    {
        isTextEditorLoaded = true;
    }

    private async void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            PlayerManager.LocalPlayer.StopInteractingWithUI();
            textEditorData.OnSelectedFileChanged -= OnFileSelectedHandlerAsync;
        }
        if(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.LeftControl))
        {
           await SaveChangesAsync();
        }

         // Check if the text has changed
        if (isTextEditorLoaded&&textEditor != null && textEditor.Text != previousText )
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

        print(data.OwnerUsername);
        textEditorHolder.SetActive(true);

        textEditorData = data;
        string text = data.DisplayText.ToString();
        textEditor.CaretPosition = new InGameTextEditor.TextPosition(0, 0);
        textEditorData.OnSelectedFileChanged += OnFileSelectedHandlerAsync;

        textEditor.SetText(text);
        directoryManager.GenerateForDirectoy(fileManagerContent.transform,textEditorData.WorkingDirectory.Value, textEditorData);
        OnTextEditorLoaded?.Invoke(this, EventArgs.Empty);

        if (textEditorData.PathToTheSelectedFile == "")
        {
            string[] files = Directory.GetFiles(textEditorData.WorkingDirectory.Value);
            if (files.Length > 0)
            {
                textEditorData.PathToTheSelectedFile = files[0];
            }
        }
    }
    public async Task SaveChangesAsync()
    {
        if (textEditorData != null && textEditor != null)
        {
            var filePath = textEditorData.PathToTheSelectedFile.ToString();
            var text = textEditor.Text;
            await File.WriteAllTextAsync(filePath, text);
        }
    }
    public void ShowGitManager()
    {
        gitManagerUi.Show();
    }
}
