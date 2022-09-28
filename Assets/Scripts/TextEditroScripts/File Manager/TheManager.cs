using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TheManager : MonoBehaviour
{
    private string path;
    public string[] files;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if(path != null && path != "")
        {
            GameObject parent = GameObject.Find("FileManager");
            int countOfParentObjs = parent.transform.childCount;
            files = Directory.GetFiles(path);

            for (int i = 0; i <= files.Length; i++)
            {
                var file = files[i];

                if (countOfParentObjs == 0 || parent.transform.GetChild(i).gameObject.name != file)
                {
                    GameObject go = new GameObject(Path.GetFileName(file));
                    go.transform.SetParent(this.transform);
                    print(file);
                    countOfParentObjs++;
                }
                if (countOfParentObjs - 1 <= i)
                {
                    i--;
                }

            }

        }
       
    }


   public void UpdatePath(string path = "")
    {
        this.path = path;

    }
}
