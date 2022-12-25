using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class TheManager : MonoBehaviour
{
    private string path;
    public string[] files;
    [SerializeField] private GameObject fileManager;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if(path != null && path != "")
        {
            GameObject parent = fileManager;
            int countOfParentObjs = parent.transform.childCount;
            files = Directory.GetFiles(path);


            foreach (string file in files)
            { 
                if (parent.transform.Find(Path.GetFileName(file)) ==null)
                {
                    
                    GameObject go = new GameObject(Path.GetFileName(file));
                    go.transform.parent = parent.transform;
                    go.transform.localPosition = new Vector3(0,0,1f);  

                    TextMeshProUGUI textEl = go.AddComponent<TextMeshProUGUI>();

                    textEl.fontSize = 4;
                    textEl.text = Path.GetFileName(file);
                }
                else
                {
                    continue;
                }
            }

        }
       
    }


   public void UpdatePath(string path = "")
    {
        this.path = path;
    }
}
