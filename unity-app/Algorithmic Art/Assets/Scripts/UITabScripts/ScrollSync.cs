using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Syncs the actual Scrolling, where as ScrollbarSync.cs syncs the position of the scrollbars.
/// 
/// </summary>
public class ScrollSync : MonoBehaviour
{
    public ScrollRect scrollRect1;
    public ScrollRect scrollRect2;

    private bool isScrolling;

    private void Start()
    {
        // Subscribe to onValueChanged events of both scroll rects
        scrollRect1.onValueChanged.AddListener(OnScrollRect1ValueChanged);
        scrollRect2.onValueChanged.AddListener(OnScrollRect2ValueChanged);
    }

    private void OnDestroy()
    {
        // Unsubscribe from events when the script is destroyed
        scrollRect1.onValueChanged.RemoveListener(OnScrollRect1ValueChanged);
        scrollRect2.onValueChanged.RemoveListener(OnScrollRect2ValueChanged);
    }

    private void OnScrollRect1ValueChanged(Vector2 value)
    {
        // Check if scrolling is already in progress to avoid recursion
        if (!isScrolling)
        {
            // Set scrolling flag to prevent infinite loop
            isScrolling = true;

            // Sync the vertical normalized position of scrollRect2 with scrollRect1
            scrollRect2.verticalNormalizedPosition = value.y;

            // Reset scrolling flag
            isScrolling = false;
        }
    }

    private void OnScrollRect2ValueChanged(Vector2 value)
    {
        // Check if scrolling is already in progress to avoid recursion
        if (!isScrolling)
        {
            // Set scrolling flag to prevent infinite loop
            isScrolling = true;

            // Sync the vertical normalized position of scrollRect1 with scrollRect2
            scrollRect1.verticalNormalizedPosition = value.y;

            // Reset scrolling flag
            isScrolling = false;
        }
    }
}