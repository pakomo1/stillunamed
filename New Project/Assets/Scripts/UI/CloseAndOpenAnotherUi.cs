using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseAndOpenAnotherUi : MonoBehaviour
{
    [SerializeField] private GameObject uiToClose;
    [SerializeField] private GameObject uiToOpen;
    [SerializeField] private GameObject Uiholder;

    public void OnClick()
    {
        uiToClose.SetActive(false);
        uiToOpen.SetActive(true);
    }
    public GameObject FindTheGameObjectOnTop(GameObject UIs)
    {
        for (int i = 0; i < UIs.transform.childCount; i++)
        {
            var childrenOfUI = UIs.transform.transform.GetChild(i).gameObject;
            for (int j = 0; j < childrenOfUI.transform.childCount; j++)
            {
                if (childrenOfUI.transform.GetChild(j).tag == "UIContainer" && childrenOfUI.transform.GetChild(j).gameObject.activeSelf)
                {
                   var objOnTop = IterateAndCheckThroughObj(childrenOfUI.transform.GetChild(j).gameObject);
                   return objOnTop;
                }
            }
        }
        return null;
    }



    public GameObject IterateAndCheckThroughObj(GameObject obj)
    {
        Stack<GameObject> stack = new Stack<GameObject>();
        GameObject topmostUIContainer = null;

        stack.Push(obj);

        while (stack.Count > 0)
        {
            GameObject current = stack.Pop();

            if (current.tag == "UIContainer" && current.activeSelf)
            {
                topmostUIContainer = current;
            }

            foreach (Transform child in current.transform)
            {
                stack.Push(child.gameObject);
            }
        }

        return topmostUIContainer;
    }
    private void Start()
    {
        FindTheGameObjectOnTop(Uiholder);
    }

}
