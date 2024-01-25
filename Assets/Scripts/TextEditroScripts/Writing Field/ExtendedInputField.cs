using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExtendedInputField : MonoBehaviour
{
   [SerializeField] private TMP_InputField inputField;

    private void Awake()
    {
        inputField.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(string value)
    {
        int cursorPosition = inputField.caretPosition;
        int lineNumber = CountLines(inputField.text, cursorPosition);
        int columnNumber = CountColumns(inputField.text, cursorPosition);
        Debug.Log("Line: " + lineNumber + ", Column: " + columnNumber);
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