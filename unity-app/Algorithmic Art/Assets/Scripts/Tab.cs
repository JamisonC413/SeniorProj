using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tab : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public TabGroup tabGroup;

    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log("click");
        tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        Debug.Log("hover");
        tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData) {
        Debug.Log("unhover");
        tabGroup.OnTabExit(this);
    }
    void Start() {
        Debug.Log("start");
        tabGroup.Subscribe(this);
    }

}
