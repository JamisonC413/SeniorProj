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
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("block");
        Debug.Log(blocks.Length);
        
        positions.Clear();
        
        positions.Add(new Vector3(0f,0f,0f));

        // Deals with snapping
        foreach (GameObject obj in blocks)
        {
            //Debug.Log(obj.GetComponent("drawBlock"));
            Vector2 move = Vector2.zero;

            if (obj.GetComponent("drawBlock") != null)
            {
                drawBlock script = (drawBlock)obj.GetComponent("drawBlock");
                positions.Add(new Vector3(script.X, script.Y, 0f));

            }
            else if (obj.GetComponent("startBlock") != null)
            {

            }
            //Vector2.Distance();
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
