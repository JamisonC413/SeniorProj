using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class mirrorBlock : NestedBlock
{

    //The brush used to get line/mesh renderer lists
    public Brush brush;

    //Initial renderer list lengths
    private int oldLineListLength = 0;
    private int oldMeshListLength = 0;

    //Flag to check if the duplicate block is currently running
    public bool flag;

    public int axis;

    // Start is called before the first frame update
    void Start()
    {
        brush = GameObject.FindGameObjectWithTag("brush").GetComponent<Brush>();
        flag = false;
    }

    public override void execute()
    {
        //Set the flag to true to indicate block is running and update initial values
        flag = true;
        oldLineListLength = brush.lineRenderers.Count;
        oldMeshListLength = brush.meshRenderers.Count;
        brush.numMirrors++;
    }

    // Update is called once per frame
    public override void updateExecute()
    {

        //When the block is running
        if (flag)
        {
      
            //Track any new line/mesh renderers
            int newLineLength = brush.lineRenderers.Count;
            int newMeshLength = brush.meshRenderers.Count;

            // Remake to use the createLineRenderer functions from Brush?

            //If there are new renderers
            if (newLineLength > oldLineListLength && brush.mirrorsDone != brush.numMirrors && brush.mirror)
            {
                //Debug.Log(brush.lRLen + " " + blockID);
                //Debug.Log(brush.lineRenderers.Count + " " + blockID);
                //brush.LRLen++;
                for(int i = oldLineListLength; i < newLineLength; i++)
                {

                    GameObject flip = Instantiate(brush.lineRenderers[i], Vector3.zero, Quaternion.identity);
                    Vector3 newDistance;

                    Vector2[] drawArea = brush.getDrawArea();
                    switch (axis)
                    {
                        case 0:
                            //flip.transform.localScale = new Vector3(brush.lineRenderers[i].transform.localScale.x, -brush.lineRenderers[i].transform.localScale.y, brush.lineRenderers[i].transform.localScale.z);
                            ////newDistance = new Vector3(0, 5f / 2, 0) - flip.transform.position;
                            ////flip.transform.position = newDistance * 2 + new Vector3(0, -1.92F, 0);
                            float yAxis = (Mathf.Abs((drawArea[0].y - drawArea[1].y)) / 2) + drawArea[0].y;
                            ////flip.transform.Translate(0f, 2.5f - (drawArea[0].y - drawArea[1].y / 2), 0f);
                            //flip.transform.Translate(0f, 2f * yAxis - flip.transform.position.y, 0f);


                            Debug.Log(drawArea[0].y + " " + drawArea[1].y + "   " + yAxis);


                            // Multiply every Vector3 in positions by newScale
                            LineRenderer lineRenderer = flip.GetComponent<LineRenderer>();
                            Vector3[] positions = new Vector3[lineRenderer.positionCount];
                            lineRenderer.GetPositions(positions);

                            for (int j = 0; j < positions.Length; j++)
                            {
                                positions[j] = new Vector3(positions[j].x, positions[j].y + 2f * (yAxis - positions[j].y), positions[j].z);
                            }

                            // Set the modified positions back to the LineRenderer
                            lineRenderer.SetPositions(positions);

                            break;
                        case 1:
                            //flip.transform.localScale = new Vector3(-brush.lineRenderers[i].transform.localScale.x, brush.lineRenderers[i].transform.localScale.y, brush.lineRenderers[i].transform.localScale.z);
                            ////newDistance = new Vector3(5f / 2, 0, 0) - flip.transform.position;
                            ////flip.transform.position = newDistance * 2 + new Vector3(7.63F, 0, 0);
                            //Debug.Log(drawArea[0].y + " " + drawArea[1].y);
                            //flip.transform.Translate((drawArea[0].y + drawArea[1].y), 0f, 0f);
                            float xAxis = (Mathf.Abs((drawArea[0].x - drawArea[1].x)) / 2) + drawArea[0].x;

                            lineRenderer = flip.GetComponent<LineRenderer>();
                            positions = new Vector3[lineRenderer.positionCount];
                            lineRenderer.GetPositions(positions);

                            for (int j = 0; j < positions.Length; j++)
                            {
                                positions[j] = new Vector3(positions[j].x + 2f * (xAxis - positions[j].x), positions[j].y, positions[j].z);
                            }

                            // Set the modified positions back to the LineRenderer
                            lineRenderer.SetPositions(positions);
                            break;

                        default: break;

                    }

                    Renderer rendererComponent = flip.GetComponent<Renderer>();
                    if (rendererComponent != null)
                    {
                        rendererComponent.sortingLayerName = "ImageRendering";
                        rendererComponent.sortingOrder = 1;
                    }

                    brush.lineRenderers.Add(flip);
                }
                brush.mirrorsDone++;
                oldLineListLength = brush.lineRenderers.Count + (int)Mathf.Pow(2, brush.mirrorsDone - brush.numMirrors);

            }


            if (newMeshLength > oldMeshListLength)
            {
                GameObject flip = Instantiate(brush.meshRenderers[newMeshLength - 1], brush.meshRenderers[newMeshLength - 1].transform.position, Quaternion.identity);
                float newDistance;
                switch (axis)
                {
                    case 0:
                        flip.transform.localScale = new Vector3(1, -1, 1);
                        newDistance = 5F - flip.transform.position.y;
                        flip.transform.position = new Vector3(flip.transform.position.x, newDistance - 1.92F, flip.transform.position.z);
                        break;
                    case 1:
                        flip.transform.localScale = new Vector3(-1, 1, 1);
                        newDistance = 5f - flip.transform.position.x;
                        flip.transform.position = new Vector3(newDistance + 7.63F, flip.transform.position.y, flip.transform.position.z);
                        break;

                    default: break;

                }

                Renderer rendererComponent = flip.GetComponent<Renderer>();
                if (rendererComponent != null)
                {
                    rendererComponent.sortingLayerName = "ImageRendering";
                    rendererComponent.sortingOrder = 1;
                }

                brush.meshRenderers.Add(flip);
                oldMeshListLength = brush.meshRenderers.Count;
            }
        }
    }
}
