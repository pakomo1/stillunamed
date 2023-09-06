using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenEscapeMenu : MonoBehaviour
{
    [SerializeField] private GameObject ui;
    private bool active;
    // Update is called once per frame

  

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ui.SetActive(!ui.activeSelf);
        }
        
    }
}
