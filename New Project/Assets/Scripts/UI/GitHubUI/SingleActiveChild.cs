using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class SingleActiveChild : MonoBehaviour
{
    private GameObject[] children;
    [SerializeField]private SideBarManagerUI sideBarManagerUI;


    void Awake()
    {
        // Get all child GameObjects
        children = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i).gameObject;
        }
    }

    // Call this method with the index of the child you want to activate with check for sideBarUI
    public void ActivateChild(int index)
    {
        for (int i = 0; i < children.Length; i++)
        {
            children[i].SetActive(checkIndex(i,index));
        }
    }
  
    private bool checkIndex(int index1, int chosenIndex)
    {
        if (index1 == chosenIndex)
        {
            if(children[index1].name.ToLower() == "repositorycontentui")
            {
                sideBarManagerUI.Show();
            }
            else
            {
                sideBarManagerUI.Hide();
            }
            return true;
        }
        return false;
    }
    //Call this method with the index of the child you want to activate
    public void ActivateOneChild(int index)
    {
        for (int i = 0; i < children.Length; i++)
        {
            children[i].SetActive(i == index);
        }
    }
}
