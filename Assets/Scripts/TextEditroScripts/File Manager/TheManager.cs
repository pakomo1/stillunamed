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

           foreach(string file in files)
            { 
                if (parent.transform.Find(Path.GetFileName(file)) ==null)
                {
                    GameObject go = new GameObject(Path.GetFileName(file));
                    go.transform.SetParent(this.transform);

                    TextMeshPro textEl = go.AddComponent<TextMeshPro>();

                    textEl.text = Path.GetFileName(file);
                    print(file);

                    //X -71
                    //Y 388
                    //Z -1
                    Vector3 vector3 = new Vector3(-71, 388, -1);
                    go.transform.position = vector3;
                    print(go.transform.position);
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
