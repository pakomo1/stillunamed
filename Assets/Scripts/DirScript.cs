using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DirScript : OpenFolder
{
    private string path1;
    [SerializeField] private TextMeshProUGUI Text;
    // Start is called before the first frame update
    void Start()
    {
        Text = FindObjectOfType<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        this.path1 = base.path;  
        Text.text = path1;
    }
}
