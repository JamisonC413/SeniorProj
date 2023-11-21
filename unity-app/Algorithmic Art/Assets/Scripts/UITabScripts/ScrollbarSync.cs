using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarSync : MonoBehaviour
{
    public ScrollRect scrollRect1;
    public ScrollRect scrollRect2;


    private void Awake()
    {
        // Ensure both scroll rects are initialized
        if (scrollRect1 == null || scrollRect2 == null)
        {
            Debug.LogError("ScrollRects are not assigned!");
            enabled = false;
            return;
        }
    }

    private void Update()
    {


    }

    public void SyncScrollRectsMax()
    {

        // Sync vertical scrollbar value of scrollRect2 to scrollRect1
        float scrollValue = scrollRect1.verticalNormalizedPosition;
        scrollRect2.verticalNormalizedPosition = scrollValue;

    }

    public void SyncScrollRectsMin()
    {

        // Sync vertical scrollbar value of scrollRect2 to scrollRect1
        float scrollValue = scrollRect2.verticalNormalizedPosition;
        scrollRect1.verticalNormalizedPosition = scrollValue;
    }
}
