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

    public void SetTheObjectOnTopToInactive(GameObject objOnTop)
    {
        
    }

    private GameObject IterateAndCheckThroughObj(GameObject uiContainer)
    {
           for (int n = 0; n < uiContainer.transform.childCount; n++)
            {
                var gameObjectChild = uiContainer.transform.GetChild(n).gameObject;

                if (gameObjectChild.tag == "UIContainer" && gameObject.activeSelf)
                {
                   uiContainer = IterateAndCheckThroughObj(gameObjectChild);
                }
                else
                {
                    continue;
                }
            }
            return uiContainer;
    }
    private void Start()
    {
        FindTheGameObjectOnTop(Uiholder);
    }

}
