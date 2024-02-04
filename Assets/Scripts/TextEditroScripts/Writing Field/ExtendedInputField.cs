using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExtendedInputField : MonoBehaviour
{
   [SerializeField] private TMP_InputField inputField;

    private void Awake()
    {
        //inputField.onValueChanged.AddListener(OnValueChanged);
       // LangaueClient.StartServer("\"C:\\Users\\Maixm\\Downloads\\omnisharp-win-x64\\OmniSharp.exe\"");

    }

    private async void OnValueChanged(string value)
    {
        int cursorPosition = inputField.caretPosition;
        int lineNumber = CountLines(inputField.text, cursorPosition);
        int columnNumber = CountColumns(inputField.text, cursorPosition);
        Debug.Log("Line: " + lineNumber + ", Column: " + columnNumber);

        //Send completion requests to the server   
       
        string method = "textDocument/completion";
        object parameters = new
        {
            textDocument = new
            {
                uri = "C:\\Users\\Maixm\\Desktop\\testFile.cs"
            },
            position = new
            {
                line = lineNumber,
                character = columnNumber
            }
        };

        await LangaueClient.SendRequest(method,parameters);
        var response = await LangaueClient.ReadResponse();
        print("Server response: " + response);
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