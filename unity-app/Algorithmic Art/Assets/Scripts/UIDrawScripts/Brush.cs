using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Brush : MonoBehaviour
{
    // Tracks the hieght and width of the rectangular draw area. Used to 
    // contain the brush to a specific area
    public Vector2 drawArea = new Vector2(10f, 10f);

    // The starting position of the brush (to reset between hitting play)
    public Vector3 startPosition;

    public List<GameObject> lineRenderers = new List<GameObject>();

    private float LineRendererID = 0;

    public GameObject LineRendererPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // Save the brush location
        startPosition = gameObject.transform.position;
        lineRenderers.Clear();
    }

    public void resetPosition()
    {
        // Start brush at the start position
        gameObject.transform.position = startPosition;
    }

    public LineRenderer createLineRenderer()
    {
        // Create an empty GameObject as a child of the brush
        LineRendererID++;
        Debug.Log("start pos : " + startPosition);
        GameObject newLineObject = Instantiate(LineRendererPrefab, startPosition, Quaternion.identity);
        
        //GameObject newLineObject = new GameObject("LineRenderer-" + LineRendererID);
        //newLineObject.transform.parent = transform;

        // Set the local position of the new GameObject to (0, 0, 0) relative to the parent (brush)
        //newLineObject.transform.localPosition = Vector3.zero;
        //newLineObject.transform.position = gameObject.transform.position;

        // Add LineRenderer to the new GameObject
        //LineRenderer lineRenderer = newLineObject.AddComponent<LineRenderer>();
        lineRenderers.Add(newLineObject);

        return newLineObject.GetComponent<LineRenderer>();
    }

    public void clearLineRenderers()
    {
        // Iterate through each LineRenderer in the list
        foreach (GameObject lineRenderer in lineRenderers)
        {
            // Destroy the GameObject associated with the LineRenderer
            Destroy(lineRenderer);
        }

        // Clear the list
        lineRenderers.Clear();
    }
}
