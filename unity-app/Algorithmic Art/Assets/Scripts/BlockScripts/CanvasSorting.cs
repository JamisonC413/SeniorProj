using UnityEngine;

public class CanvasSorting : MonoBehaviour
{
    public Canvas subCanvas;
    public string sortingLayerName = "Text";
    public int sortingOrder = 3;

    void Start()
    {
        //Canvas canvas = GetComponent<Canvas>();

        // Check if the child canvas has a Canvas component
        if (subCanvas != null)
        {
            // Set the sorting layer and order
            subCanvas.overrideSorting = true;
            subCanvas.gameObject.layer = 7;
            subCanvas.sortingLayerName = sortingLayerName;
            subCanvas.sortingOrder = sortingOrder;
        }
        else
        {
            Debug.LogWarning("Child object does not have a Canvas component.");
        }
    }
}