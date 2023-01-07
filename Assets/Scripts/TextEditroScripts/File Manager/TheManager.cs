using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class TheManager : MonoBehaviour
{
    private string path;
    public string[] files;
    public string[] folders;
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
            
            folders = Directory.GetDirectories(path);
            foreach (string folder in folders)
            {
                if (parent.transform.Find(Path.GetFileName(folder)) == null)
                {
                    GameObject go = new GameObject(Path.GetFileName(folder));
                    go.transform.SetParent(parent.transform);
                    go.transform.localPosition = new Vector3(0, 0, 1f);
                    go.tag = "directory";

                    TextMeshProUGUI textEl = go.AddComponent<TextMeshProUGUI>();
                    textEl.fontSize = 4;
                    textEl.text = Path.GetFileName(folder);
                    textEl.color = Color.red;

                    VerticalLayoutGroup vlg = go.AddComponent<VerticalLayoutGroup>();
                    vlg.childForceExpandHeight = false;
                    vlg.padding.left = 2;
                    vlg.padding.top = 4;

                    string[] files = Directory.GetFiles(folder);
                    foreach (string file in files)
                    {
                        GameObject fileGO = new GameObject(Path.GetFileName(file));
                        fileGO.transform.SetParent(go.transform);
                        fileGO.transform.localPosition = new Vector3(0, 0, 1f);
                        fileGO.tag = "file";

                        TextMeshProUGUI FiletextEl = fileGO.AddComponent<TextMeshProUGUI>();
                        FiletextEl.fontSize = 4;
                        FiletextEl.text = Path.GetFileName(file);
                        FiletextEl.color = Color.blue;
                    }
                }
            }
            
            files = Directory.GetFiles(path);

            foreach (string file in files)
            { 
                if (parent.transform.Find(Path.GetFileName(file)) ==null)
                {
                    
                    GameObject go = new GameObject(Path.GetFileName(file));
                    go.transform.parent = parent.transform;
                    go.transform.localPosition = new Vector3(0,0,1f);
                    go.tag = "file";

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
