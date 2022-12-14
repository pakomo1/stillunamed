using UnityEngine;
using TMPro;

public class DirScript : MonoBehaviour
{
    [SerializeField] private OpenFolder openFolder; 
    [SerializeField] private TextMeshProUGUI text;
    // Start is called before the first frame update
    private string path;
    void Start()
    {
       
    }
    
    // Update is called once per frame
    void Update()
    {
        path = openFolder.path;

        if (path.Length > 24)
        {
            string newText = path.Substring(24);
            text.text = newText + "...";
        }
    }
}
