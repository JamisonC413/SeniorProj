using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tab : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public Palette palette;

    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log("click");
        palette.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        Debug.Log("hover");
        palette.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData) {
        Debug.Log("unhover");
        palette.OnTabExit(this);
    }
    void Start() {
        Debug.Log("start");
        palette.Subscribe(this);
    }

}
