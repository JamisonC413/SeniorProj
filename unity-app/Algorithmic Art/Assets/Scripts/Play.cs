using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private List<Vector3> positions = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        // lineRenderer = GetComponent<LineRenderer>();
    }

    public void Render()
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("block");
        Debug.Log(blocks.Length);
        
        positions.Clear();
        
        positions.Add(new Vector3(0f,0f,0f));

        for (int i = 0; i < blocks.Length; i++)
        {
            // Add the position of each block to the positions list.
            positions.Add(new Vector3(i * 1f, 0f, 0f));
        }

        // Assign the positions to the LineRenderer.
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }

    // Update is called once per frame
    void Update()
    {
    }
}
