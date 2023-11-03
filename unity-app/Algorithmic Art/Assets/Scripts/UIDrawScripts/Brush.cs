using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Brush : MonoBehaviour
{
    // Tracks the hieght and width of the rectangular draw area. Used to 
    // contain the brush to a specific area
    public Vector2 drawArea = new Vector2(5f, 5f);

    // The starting position of the brush (to reset between hitting play)
    public Vector3 startPosition;

    public List<GameObject> lineRenderers = new List<GameObject>();

    public List<GameObject> meshRenderers = new List<GameObject>();

    private float LineRendererID = 0;
    private float MeshRendererID = 0;

    public GameObject LineRendererPrefab;

    public GameObject MeshRendererPrefab;
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
        GameObject newLineObject = Instantiate(LineRendererPrefab, startPosition, Quaternion.identity, gameObject.transform);
        newLineObject.name = "Line Renderer" + LineRendererID;

        // Add LineRenderer to the new GameObject
        lineRenderers.Add(newLineObject);

        return newLineObject.GetComponent<LineRenderer>();
    }

    public MeshRenderer createMeshRenderer()
    {
        // Create an empty GameObject as a child of the brush
        MeshRendererID++;
        Debug.Log("start pos : " + startPosition);
        GameObject newMeshObject = Instantiate(MeshRendererPrefab, gameObject.transform.position, Quaternion.identity);
        newMeshObject.name = "Mesh Renderer" + MeshRendererID;

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

}
