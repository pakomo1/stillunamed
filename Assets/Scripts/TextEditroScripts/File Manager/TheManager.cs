using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class TheManager : MonoBehaviour
{
    private string currentWorkingDir;
    public string[] files;
    public string[] folders;
    [SerializeField] private GameObject fileManager;
    [SerializeField] private GameObject TextFieldManager;
    [SerializeField] private TextEditor textEditor;
    private OpenFolder openfolder;
    // Start is called before the first frame update
    void Start()
    {
        openfolder = TextFieldManager.GetComponent<OpenFolder>();
    }

    // Update is called once per frame
    void Update()
    {

        if(currentWorkingDir != null && currentWorkingDir != "")
        {
            GameObject parent = fileManager;
            int countOfParentObjs = parent.transform.childCount;
            
            folders = Directory.GetDirectories(currentWorkingDir);
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
            
            files = Directory.GetFiles(currentWorkingDir);

            foreach (string file in files)
            { 
                if (parent.transform.Find(Path.GetFileName(file)) ==null)
                {
                    GameObject go = new GameObject(Path.GetFileName(file));
                    go.transform.parent = parent.transform;
                    go.transform.localPosition = new Vector3(0,0,1f);
                    go.tag = "file";
                    var data = go.GetComponent<FilesMetaData>();
                    //data.IsSelected = false;
                    
                    go.AddComponent<Button>();
                    Button btn = go.GetComponent<Button>();
                    btn.onClick.AddListener(delegate { OnClickFile.OnPointerClick(go, textEditor); });

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


}
