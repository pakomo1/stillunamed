using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenEscapeMenu : MonoBehaviour
{
    [SerializeField] private GameObject ui;
    private bool active;
    // Update is called once per frame

    private void Awake()
    {
        active = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && active == false)
        {
            ui.SetActive(true);
            active = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && active == true)
        {
            ui.SetActive(false);
            active = false;
        }
    }
}
