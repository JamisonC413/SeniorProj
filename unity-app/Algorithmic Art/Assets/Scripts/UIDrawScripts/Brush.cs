using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Brush : MonoBehaviour
{
    // Tracks the hieght and width of the rectangular draw area. Used to 
    // contain the brush to a specific area
    [SerializeField]
    private Vector2 drawArea = new Vector2(5f, 5f);

    // The starting position of the brush (to reset between hitting play)
    public Vector3 startPositionMinimized;

    public Vector3 startPositionMaximized;

    public GameObject startPos2;

    public List<GameObject> lineRenderers = new List<GameObject>();

    public List<GameObject> meshRenderers = new List<GameObject>();

    private float LineRendererID = 0;
    private float MeshRendererID = 0;

    public GameObject LineRendererPrefab;

    public GameObject MeshRendererPrefab;

    private bool isMaximized;

    [SerializeField]
    private float maximizedScale;

    // Start is called before the first frame update
    void Start()
    {
        isMaximized = false;
        // Save the brush location
        startPositionMinimized = gameObject.transform.position;
        startPositionMaximized = startPos2.transform.position;
        lineRenderers.Clear();
    }

    public Vector2[] getDrawArea()
    {
        Vector2[] output = new Vector2[2];
        if(isMaximized)
        {
            output[0] = startPositionMaximized;
            output[1] = drawArea + (Vector2)startPositionMaximized;
        }
        else
        {
            output[0] = startPositionMinimized;
            output[1] = drawArea + (Vector2)startPositionMinimized;
        }
        return output;
    }

    public void resetPosition()
    {
        if(isMaximized)
        {
            gameObject.transform.position = startPositionMaximized;
        }
        else
        {
            // Start brush at the start position
            gameObject.transform.position = startPositionMinimized;
        }
    }

    public LineRenderer createLineRenderer()
    {
        // Create an empty GameObject as a child of the brush
        LineRendererID++;
        //Debug.Log("start pos : " + startPosition);
        GameObject newLineObject = Instantiate(LineRendererPrefab, Vector3.zero, Quaternion.identity);
        newLineObject.name = "Line Renderer" + LineRendererID;

        Renderer rendererComponent = newLineObject.GetComponent<Renderer>();
        if (rendererComponent != null)
        {
            rendererComponent.sortingLayerName = "ImageRendering";
            rendererComponent.sortingOrder = 1;
        }

        // Add LineRenderer to the new GameObject
        lineRenderers.Add(newLineObject);

        return newLineObject.GetComponent<LineRenderer>();
    }

    public MeshRenderer createMeshRenderer()
    {
        // Create an empty GameObject as a child of the brush
        MeshRendererID++;
        //Debug.Log("start pos : " + startPosition);
        GameObject newMeshObject = Instantiate(MeshRendererPrefab, gameObject.transform.position, Quaternion.identity);
        newMeshObject.name = "Mesh Renderer" + MeshRendererID;

        Renderer rendererComponent = newMeshObject.GetComponent<Renderer>();
        if (rendererComponent != null)
        {
            rendererComponent.sortingLayerName = "ImageRendering";
            rendererComponent.sortingOrder = 1;
        }

        // Add LineRenderer to the new GameObject
        meshRenderers.Add(newMeshObject);

        return newMeshObject.GetComponent<MeshRenderer>();
    }

    public void clearRenderers()
    {
        // Iterate through each LineRenderer in the list
        foreach (GameObject lineRenderer in lineRenderers)
        {
            // Destroy the GameObject associated with the LineRenderer
            Destroy(lineRenderer);
        }

        // Iterate through each LineRenderer in the list
        foreach (GameObject meshRenderer in meshRenderers)
        {
            // Destroy the GameObject associated with the LineRenderer
            Destroy(meshRenderer);
        }

        // Clear the lists
        lineRenderers.Clear();
        meshRenderers.Clear();
    }

    public void moveDrawing()
    {
        if(isMaximized)
        {
            Vector3 translate = new Vector3(-27, 0, 0);
            isMaximized = false;
            float minimizedScale = 1 / maximizedScale;
            transformRenderers(minimizedScale, translate, startPositionMinimized, startPositionMaximized);
        }
        else
        {
            Vector3 translate = new Vector3(27, 0, 0);
            isMaximized = true;
            transformRenderers(maximizedScale, translate, startPositionMaximized, startPositionMinimized);
        }
    }

    private void transformRenderers(float scale, Vector3 translate, Vector3 newOrigin, Vector3 oldOrigin)
    {
        Vector3 tempDifference;
        // Iterate through each LineRenderer in the list
        foreach (GameObject lineRendererObject in lineRenderers)
        {
            // Get the LineRenderer component from the child GameObject
            LineRenderer lineRenderer = lineRendererObject.GetComponent<LineRenderer>();

            if (lineRenderer != null)
            {

                // Multiply every Vector3 in positions by newScale
                Vector3[] positions = new Vector3[lineRenderer.positionCount];
                lineRenderer.GetPositions(positions);

                for (int i = 0; i < positions.Length; i++)
                {
                    //Vector3 difference = positions[i] - startPositionMinimized;
                    //positions[i] = (difference * scale) + startPositionMinimized;
                    //positions[i] = positions[i] + translate;
                    tempDifference = positions[i] - oldOrigin;
                    tempDifference = tempDifference * scale;
                    positions[i] = newOrigin + tempDifference;
                }

                // Set the modified positions back to the LineRenderer
                lineRenderer.SetPositions(positions);
            }
            else
            {
                Debug.LogWarning("LineRenderer component not found on child GameObject.");
            }
        }

        // Iterate through each MeshRenderer in the list
        foreach (GameObject meshRendererObject in meshRenderers)
        {
            // Change the x, y, and z scale
            meshRendererObject.transform.localScale = scale * meshRendererObject.transform.localScale;
            tempDifference = meshRendererObject.transform.position - oldOrigin;
            tempDifference = tempDifference * scale;        
            meshRendererObject.transform.position = newOrigin + tempDifference;
        }

        tempDifference = gameObject.transform.position - oldOrigin;
        tempDifference = tempDifference * scale;
        gameObject.transform.position = newOrigin + tempDifference;
    }

}
