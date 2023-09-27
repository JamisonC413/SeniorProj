using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private List<Vector3> positions = new List<Vector3>();
    public startBlock startScript;

    // Start is called before the first frame update
    void Start()
    {
        // lineRenderer = GetComponent<LineRenderer>();
    }

    public void Render()
    {
        positions.Clear();

        positions.Add(new Vector3(0f, 0f, 0f));

        Block block = startScript.nextBlock;
        while (block != null)
        {
            positions.Add(new Vector3(((drawBlock)block).X, ((drawBlock)block).Y, 0f));
            block = block.nextBlock;
        }

        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }

    // Update is called once per frame
    void Update()
    {
    }
}
