using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    [SerializeField] private GameObject fileManagerContent;
    private GenerateForDirectory generateForDirectory;
    // Start is called before the first frame update
    void Start()
    {
        generateForDirectory =gameObject.GetComponent<GenerateForDirectory>();
        generateForDirectory.GenerateForDirectoy(fileManagerContent.transform, @"C:\Users\Maixm\Desktop\DZI");
    }

    // Update is called once per frame
    void Update()
    {
    }
}
