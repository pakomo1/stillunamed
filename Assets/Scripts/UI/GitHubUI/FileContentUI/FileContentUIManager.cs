using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FileContentUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pathToFile;



    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
