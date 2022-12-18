using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

public class DirScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private OpenFolder openFolder;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject holeCurrentDir;

    private TextMeshProUGUI holeCurrentDirText;
    // Start is called before the first frame update
    private string path;
    private bool isTooLong;
    void Start()
    {
        holeCurrentDirText = holeCurrentDir.GetComponent<TextMeshProUGUI>();  
    }

    // Update is called once per frame
    void Update()
    {
        path = openFolder.path;
        holeCurrentDirText.text = path;
        
        if (path.Length > 24)
        {
            isTooLong = true;

            string newText = path.Substring(24);
            text.text = newText + "...";
        }

    }
  
    public void  OnPointerEnter(PointerEventData eventData)
    {
        if(path != "" && isTooLong)
        {
            holeCurrentDir.SetActive(true);   
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        holeCurrentDir.SetActive(false);
    }
}
