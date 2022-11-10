using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    List<TabButton> tabButtons;
    public Color tabIdle; 
    public Color tabHover; 
    public Color tabActive;

    public TabButton selectedTab;
    public List<GameObject> obejctsToSwap; 
    public void Subscribe(TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }
        tabButtons.Add(button); 
    }
    public void onTabEnter(TabButton button)
    {

        ResetTabs();
        if(selectedTab == null || button != selectedTab)
        {
           button.background.color = tabHover;
        }
    }
    public void onTabExit(TabButton button)
    {
        ResetTabs();
    }
    public void onTabSelected(TabButton button)
    {
       selectedTab = button;
        ResetTabs();
        button.background.color = tabActive;

        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < obejctsToSwap.Count; i++)
        {
            if(i == index)
            { 
                obejctsToSwap[i].SetActive(true);
            }
            else
            {
                obejctsToSwap[i].SetActive(false);   
            }
        }
    }
    public void ResetTabs()
    {
        foreach(TabButton button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab)
            {
                continue;
            }
            button.background.color = tabIdle;
        }
    }
}
