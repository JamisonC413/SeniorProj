using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mirrorBlockBottom : nestedBottom
{ 
    public override void execute()
    {
        mirrorBlock mirror = ((mirrorBlock)topBlock);

        //Track any new line/mesh renderers
        int newLineLength = mirror.brush.lineRenderers.Count;
        int newMeshLength = mirror.brush.meshRenderers.Count;

        // Remake to use the createLineRenderer functions from Brush?

        //For each new line renderer
        for (int i = mirror.oldLineListLength; i < newLineLength; i++)
        {
            //Make a new line renderer to flip
            GameObject flip = Instantiate(mirror.brush.lineRenderers[i], Vector3.zero, Quaternion.identity);

            Vector2[] drawArea = mirror.brush.getDrawArea();
            //Mirror across the correct axis
            switch (mirror.axis)
            {
                //Across X axis
                case 0:
                    float xAxis = (Mathf.Abs((drawArea[0].y - drawArea[1].y)) / 2) + drawArea[0].y;

                    // Multiply every Vector3 in positions by newScale
                    LineRenderer lineRenderer = flip.GetComponent<LineRenderer>();
                    Vector3[] positions = new Vector3[lineRenderer.positionCount];
                    lineRenderer.GetPositions(positions);

                    //Adjust each position to the mirrored value
                    for (int j = 0; j < positions.Length; j++)
                    {
                        positions[j] = new Vector3(positions[j].x, positions[j].y + 2f * (xAxis - positions[j].y), positions[j].z);
                    }

                    // Set the modified positions back to the LineRenderer
                    lineRenderer.SetPositions(positions);
                    break;

                //Across Y axis
                case 1:

                    float yAxis = (Mathf.Abs((drawArea[0].x - drawArea[1].x)) / 2) + drawArea[0].x;

                    lineRenderer = flip.GetComponent<LineRenderer>();
                    positions = new Vector3[lineRenderer.positionCount];
                    lineRenderer.GetPositions(positions);

                    for (int j = 0; j < positions.Length; j++)
                    {
                        positions[j] = new Vector3(positions[j].x + 2f * (yAxis - positions[j].x), positions[j].y, positions[j].z);
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

            mirror.brush.lineRenderers.Add(flip);
        }

        //For each new mesh renderer
        for (int i = mirror.oldMeshListLength; i < newMeshLength; i++)
        {
            //Make a new mesh renderer to flip
            GameObject flip = Instantiate(mirror.brush.meshRenderers[i], mirror.brush.meshRenderers[i].transform.position, Quaternion.identity);

            Vector2[] drawArea = mirror.brush.getDrawArea();
            //Mirror across the correct axis
            switch (mirror.axis)
            {
                //Across X axis
                case 0:
                    float xAxis = (Mathf.Abs((drawArea[0].y - drawArea[1].y)) / 2) + drawArea[0].y;

                    flip.transform.localScale = new Vector3(flip.transform.localScale.x, -flip.transform.localScale.y, flip.transform.localScale.z);

                    flip.transform.Translate(0f, (xAxis - flip.transform.position.y) * 2, 0f);
                    //float newDistance = 5F - flip.transform.position.y;
                    //flip.transform.position = new Vector3(flip.transform.position.x, newDistance - 1.92F, flip.transform.position.z);
                    break;
                //Across Y axis
                case 1:

                    float yAxis = (Mathf.Abs((drawArea[0].x - drawArea[1].x)) / 2) + drawArea[0].x;

                    flip.transform.localScale = new Vector3(-flip.transform.localScale.x, flip.transform.localScale.y, flip.transform.localScale.z);

                    flip.transform.Translate((yAxis - flip.transform.position.x) * 2, 0f, 0f);

                    break;

                default: break;

            }

            Renderer rendererComponent = flip.GetComponent<Renderer>();
            if (rendererComponent != null)
            {
                rendererComponent.sortingLayerName = "ImageRendering";
                rendererComponent.sortingOrder = 1;
            }
            mirror.brush.meshRenderers.Add(flip);

        }

    }
}
