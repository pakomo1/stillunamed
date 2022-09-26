using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using System.Linq;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;   
    [SerializeField] private float textSpeed;
    [SerializeField] private TextAsset textFile;
    [SerializeField] private int[] linesToRead;
    private int index;
    private List<string> lines = new List<string>();

    private void Start()
    {
        textComponent.text = string.Empty;       
        string[] text = textFile.text.Split("/");
        for (int i = 0; i < linesToRead.Length; i++)
        {
            lines.Add(text[linesToRead[i]]);
        }
        StartDialogue();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    private void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLines());
    }

    private IEnumerator TypeLines()
    {
        foreach (char item in lines[index].ToCharArray())   
        {
            textComponent.text += item;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void NextLine()
    {
        if (index < lines.Count - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLines());
        }
        else
        {
            gameObject.active = false;
        }
    }
}
