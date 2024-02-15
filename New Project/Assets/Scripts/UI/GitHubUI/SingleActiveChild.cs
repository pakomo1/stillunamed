using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleActiveChild : MonoBehaviour
{
    private GameObject[] children;

    void Start()
    {
        // Get all child GameObjects
        children = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i).gameObject;
        }
    }

    // Call this method with the index of the child you want to activate
    public void ActivateChild(int index)
    {
        for (int i = 0; i < children.Length; i++)
        {
            children[i].SetActive(i == index);
        }
    }
}
