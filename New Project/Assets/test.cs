using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private IssueUIManager issueManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            mainMenu.SetActive(true);
            issueManager.Show();
        }
    }
}
