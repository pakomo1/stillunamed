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
            float xPosition = (float)-0.00038004;
            int yPosition = 388;
            int zPosition = -1;

            foreach (string file in files)
            { 
                if (parent.transform.Find(Path.GetFileName(file)) ==null)
                {
                    GameObject go = new GameObject(Path.GetFileName(file));
                    
                    go.transform.SetParent(this.transform);

                    TextMeshPro textEl = go.AddComponent<TextMeshPro>();
                    textEl.text = Path.GetFileName(file);
                    textEl.fontSize = 40;

                    textEl.rectTransform.sizeDelta = new Vector2((float)51.591, 5); 

                    //X -71
                    //Y 388 -35
                    //Z -1
                    go.SetActive(false);

                    go.transform.localPosition = new Vector3(xPosition, yPosition, zPosition);
                    go.SetActive(true);
                    yPosition -= 35;
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
