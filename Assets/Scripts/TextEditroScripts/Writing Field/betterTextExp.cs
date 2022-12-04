using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;


public class betterTextExp : MonoBehaviour
{
    [SerializeField]TMP_InputField InputField;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame

    IEnumerator FieldFix(int saveCaretPosition)
    {
        yield return null;
        InputField.MoveTextEnd(false);

        InputField.caretPosition = saveCaretPosition;
        InputField.text = InputField.text.Insert(InputField.caretPosition, "\n");
        InputField.caretPosition = saveCaretPosition + 1;
    }
    void Update()
    {
        int saveCaretPosition = InputField.caretPosition;
        if (EventSystem.current.currentSelectedGameObject == InputField.gameObject)
        {
            if ( Input.GetKeyUp(KeyCode.Return))
            {
                InputField.ActivateInputField();
                StartCoroutine(FieldFix(saveCaretPosition));
            }
        }
    }
}

