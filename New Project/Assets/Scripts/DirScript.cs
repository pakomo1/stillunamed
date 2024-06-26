using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class DirScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private OpenFolder openFolder;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject holeCurrentDir;
    [SerializeField] private GameObject image;

    [SerializeField] private TextEditorData textEditor;
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
        path = textEditor.WorkingDirectory.Value; 
        holeCurrentDirText.text = path;
        
        if (path.Length > 24 && path != null)
        {
            isTooLong = true;

            string newText = path.Substring(0,20);
            text.text = newText + "...";
        }

    }
  

    IEnumerator Act()
    {
        yield return new WaitForSeconds(2.0f);
        image.SetActive(true);
        holeCurrentDir.SetActive(true);
    }
    public void  OnPointerEnter(PointerEventData eventData)
    {
        if(path != "" && isTooLong && !image.activeSelf) 
        {
            StartCoroutine(Act());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.SetActive(false);
        holeCurrentDir.SetActive(false);
    }
}
