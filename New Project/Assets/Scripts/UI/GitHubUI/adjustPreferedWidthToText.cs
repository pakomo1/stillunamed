using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class adjustPreferedWidthToText : MonoBehaviour
{
    private TextMeshProUGUI text;

    void Start()
    {
        text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        var layoutElement = GetComponent<LayoutElement>();
        layoutElement.preferredWidth = text.preferredWidth;
    }
}
