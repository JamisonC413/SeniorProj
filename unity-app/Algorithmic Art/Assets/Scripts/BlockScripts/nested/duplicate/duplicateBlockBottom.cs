using Codice.Client.Common.GameUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

public class duplicateBlockBottom : nestedBottom
{ 
    public override void execute()
    {
        duplicateBlock dup = ((duplicateBlock)topBlock);

        int angleSize = 360 / (dup.duplicate + 1);

        //Track any new line/mesh renderers
        int newLineLength = dup.brush.lineRenderers.Count;
        int newMeshLength = dup.brush.meshRenderers.Count;

        //For each new line renderer
        for (int i = dup.oldLineListLength; i < newLineLength; i++)
        {
            if (newLineLength > dup.oldLineListLength)
            {
                //Make # of duplicates specified
                for (int j = 0; j < dup.duplicate; j++)
                {
                    //Make a copy of the linerenderer 
                    GameObject rotate = Instantiate(dup.brush.lineRenderers[i], new Vector3(0, 0, 0), Quaternion.identity);

                    LineRenderer lineRenderer = rotate.GetComponent<LineRenderer>();
                    Vector3[] positions = new Vector3[lineRenderer.positionCount];
                    lineRenderer.GetPositions(positions);

                    //For each position
                    for (int k = 0; k < positions.Length; k++)
                    {
                        //Rotate the position around the starting brush point by the specified angle
                        float radianNum = angleSize * (j + 1) / 180F * Mathf.PI;
                        float sin = Mathf.Sin(radianNum);
                        float cos = Mathf.Cos(radianNum);

                        positions[k] -= new Vector3(dup.startPos.x, dup.startPos.y, 0);

                        float xnew = positions[k].x * cos - positions[k].y * sin;
                        float ynew = positions[k].x * sin + positions[k].y * cos;

                        positions[k] = new Vector3(xnew + dup.startPos.x, ynew + dup.startPos.y, positions[k].z);
                    }
                    // Set the modified positions back to the LineRenderer
                    lineRenderer.SetPositions(positions);

                    Renderer rendererComponent = rotate.GetComponent<Renderer>();
                    if (rendererComponent != null)
                    {
                        rendererComponent.sortingLayerName = "ImageRendering";
                        rendererComponent.sortingOrder = 1;
                    }

                    //Add this new line renderer to the list and increase the list count
                    dup.brush.lineRenderers.Add(rotate);
                }

            }
        }

        if (newMeshLength > dup.oldMeshListLength)
        {
            for (int i = dup.oldMeshListLength; i < newMeshLength; i++)
            {

                for (int j = 0; j < dup.duplicate; j++)
                {
                    GameObject rotate = Instantiate(dup.brush.meshRenderers[i], dup.brush.meshRenderers[i].transform.position, Quaternion.identity);
                    //Make a copy of the meshrenderer and rotate it around the starting brush position
                    rotate.GetComponent<MeshRenderer>().transform.RotateAround(dup.startPos, Vector3.forward, angleSize * (j + 1));

                    //Add this new mesh renderer to the list
                    Renderer rendererComponent = rotate.GetComponent<Renderer>();
                    if (rendererComponent != null)
                    {
                        rendererComponent.sortingLayerName = "ImageRendering";
                        rendererComponent.sortingOrder = 1;
                    }
                    dup.brush.meshRenderers.Add(rotate);
                }
               
            }
        }
    }

}
