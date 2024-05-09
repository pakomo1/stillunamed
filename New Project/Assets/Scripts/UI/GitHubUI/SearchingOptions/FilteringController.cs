using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Resources;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
using System;

public class FilteringController : MonoBehaviour
{
    [SerializeField] private ValidAccessToken validAccessToken;
    [SerializeField] private GameObject BeforeAuthUi;
    [SerializeField] private GameObject contentHolder;  
    [SerializeField] private GameObject searchBar;
    [SerializeField] private TextMeshProUGUI availabilityButton;
    private int state = 0;
    private List<GameObject> elements;

    public void GetContent()
    {
        elements = GetGameObjectContents.GetContent(contentHolder);
    }

    public void Search()
    {
        string searchText = searchBar.GetComponent<TMP_InputField>().text;
        int searchTxtLength = searchText.Length;
        foreach (GameObject item in elements)
        {
            if (item.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.Length >= searchTxtLength)
            {
                if (item.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.ToLower().Contains(searchText.ToLower()))
                {
                    item.SetActive(true);
                }
                else
                {
                    item.SetActive(false);
                }
            }
        }
        availabilityButton.text = "ALL";
        state = 2;
    }
    public void SearchForFile()
    {
        string searchText = searchBar.GetComponent<TMP_InputField>().text;
        elements = getChildren(contentHolder);
        foreach (GameObject item in elements)
        {
            if (item.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.Length >= searchText.Length)
            {
                if (item.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.ToLower().Contains(searchText.ToLower()))
                {
                    item.SetActive(true);
                }
                else
                {
                    item.SetActive(false);
                }
            }
        }
    }

    public List<GameObject> getChildren(GameObject obj)
    {
        var children = new List<GameObject>();

        for (int i = 0; i < obj.transform.childCount; i++)
        {
            var child = obj.transform.GetChild(i).gameObject;
            children.Add(child);
        }
        return children;
    }

    public void SortAvailability()
    {
        GetContent();
        if (state >= 3)
        {
            state = 0;
        }
        foreach (GameObject item in elements)
        {       
            if (state == 0)
            {
                availabilityButton.text = "PRIVATE";
                if (item.transform.GetChild(3).GetComponent<Image>().color == Color.red)
                {
                    item.SetActive(true);
                }
                else
                {
                    item.SetActive(false);
                }
            }
            else if (state == 1)
            {
                availabilityButton.text = "PUBLIC";
                if (item.transform.GetChild(3).GetComponent<Image>().color == Color.green)
                {
                    item.SetActive(true);
                }
                else
                {
                    item.SetActive(false);
                }
            }
            else if (state == 2)
            {
                availabilityButton.text = "ALL";
                item.SetActive(true);
            }
        }
        state++;
    }

    public void Refresh()
    {
        if (BeforeAuthUi.activeSelf)
        {
            BeforeAuthUi.SetActive(false);
        }
        GetContent();
        foreach (GameObject item in elements)
        {
            Destroy(item);
        }
        validAccessToken.ValidateToken();
    }

    public void Ascending()
    {
        InstantiateSortedList(false);
    }

    public void Descending()
    {
        InstantiateSortedList(true);
    }

    private void InstantiateSortedList(bool reversedList)
    {
        GetContent();
        elements.Sort(new CompareNames());
        if (reversedList)
        {
            elements.Reverse();
        }
        foreach (GameObject item in elements)
        {
            Instantiate(item, contentHolder.transform);
            Destroy(item);
        }
    }

    public class CompareNames : IComparer<GameObject>
    {
        int IComparer<GameObject>.Compare(GameObject x, GameObject y)
        {
            return string.Compare(x.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text, y.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
        }
    }
}
