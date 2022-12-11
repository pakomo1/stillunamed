using UnityEngine;
using TMPro;

public class DirScript : MonoBehaviour
{
    [SerializeField] private OpenFolder openFolder; 
    [SerializeField] private TextMeshProUGUI Text;
    // Start is called before the first frame update
    void Start()
    {

    }
    
    // Update is called once per frame
    void Update()
    {
        if(openFolder.path == "")
        {
            Text.text = "<-----------";
        }
        else
        {
            Text.text = openFolder.path;
        }
    }
}
