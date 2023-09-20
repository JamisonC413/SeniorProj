using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<Tab> tabs;

    public Tab selectedTab;

    public List<GameObject> palette;

    public void Subscribe(Tab tab) {
        if(tabs == null) {
            tabs = new List<Tab>();
        }

        tabs.Add(tab);
    }

    public void OnTabEnter(Tab tab) {
        //TODO
    }

    public void OnTabExit(Tab tab) {
        //TODO
    }

    public void OnTabSelected(Tab tab) {
        selectedTab = tab;
        int index = tab.transform.GetSiblingIndex();
        for(int i = 0; i < palette.Count; i++) {
            if (i == index) {
                palette[i].SetActive(true);
            }
            else {
                palette[i].SetActive(false);
            }
        }
    }

    public void ResetTabs() {
        //TODO
    }
}
