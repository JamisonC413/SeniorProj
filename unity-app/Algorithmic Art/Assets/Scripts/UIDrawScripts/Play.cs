using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class Play : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private List<Vector3> positions = new List<Vector3>();
    public startBlock startScript;
    public GameObject brush;
    public float delay = 1f;

    private Vector3 startBrush;
  
    // Start is called before the first frame update
    void Start()
    {
        startBrush = brush.transform.position;
        // lineRenderer = GetComponent<LineRenderer>();
    }

    public IEnumerator Render()
    {
        positions.Clear();

        positions.Add(new Vector3(0f, 0f, 0f));

        brush.transform.position = startBrush;

        Block block = startScript.nextBlock;
        float lastx = 0;
        float lasty = 0;
        while (block != null)
        {
            float blockX = ((drawBlock)block).X;
            float blockY = ((drawBlock)block).Y;
            float xTransform = lastx + blockX;
            float yTransform = lasty + blockY;

            if (xTransform < 0)
            {
                xTransform = 0;
            }
            if (yTransform < 0)
            {
                yTransform = 0;
            }
            Debug.Log(brush.transform.position);
            Debug.Log(new Vector3(xTransform, yTransform, 0f));

            positions.Add(new Vector3(xTransform, yTransform, 0f));
            block = block.nextBlock;

            lineRenderer.positionCount = positions.Count;
            lineRenderer.SetPositions(positions.ToArray());

            brush.transform.position = startBrush + new Vector3(xTransform, yTransform, 0f);

            lasty = yTransform;
            lastx = xTransform;

            yield return new WaitForSeconds(delay);
        }

        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }

    public void StartRendering()
    {
        StartCoroutine(Render());
    }

    // Update is called once per frame
    void Update()
    {
    }
}
