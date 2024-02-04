using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem.iOS;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

public class ExtendedInputField : MonoBehaviour
{
   [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextEditor textEditor;

    private void Awake()
    {
        inputField.onValueChanged.AddListener(OnValueChanged);
        LangaueClient.StartServer("C:\\Users\\Maixm\\Downloads\\omnisharp-win-x64\\OmniSharp.exe");

    }

    private async void OnValueChanged(string value)
    {
        int cursorPosition = inputField.caretPosition;
        int lineNumber = CountLines(inputField.text, cursorPosition);
        int columnNumber = CountColumns(inputField.text, cursorPosition);


       var completionItems =  await LangaueClient.RequestCompletionAsync(textEditor.PathToTheSelectedFile,lineNumber, columnNumber);
        foreach (CompletionItem item in completionItems)
        {
            print($"Completion item: {item}");
        }
    }

    private int CountLines(string text, int upto)
    {
        int lineCount = 0;
        for (int i = 0; i < upto; i++)
        {
            if (text[i] == '\n')
            {
                lineCount++;
            }
        }
        return lineCount;
    }

    private int CountColumns(string text, int upto)
    {
        int columnCount = 0;
        for (int i = upto - 1; i >= 0; i--)
        {
            if (text[i] == '\n')
            {
                break;
            }
            columnCount++;
        }
        return columnCount;
    }
}