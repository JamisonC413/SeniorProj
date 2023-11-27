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
            if (newLineLength > oldLineListLength)
            {
                GameObject flip = Instantiate(brush.lineRenderers[newLineLength - 1], new Vector3 (0,0,0), Quaternion.identity);
                Vector3 newDistance;
                switch (axis)
                {
                    case 0:
                        flip.transform.localScale = new Vector3(1, -1, 1);
                        newDistance = new Vector3(0, 5f / 2, 0) - flip.transform.position;
                        flip.transform.position = newDistance * 2 + new Vector3(0, -1.92F, 0);
                        break;
                    case 1:
                        flip.transform.localScale = new Vector3(-1, 1, 1);
                        newDistance = new Vector3(5f / 2, 0, 0) - flip.transform.position;
                        flip.transform.position = newDistance * 2 + new Vector3(7.63F, 0, 0);
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
                oldLineListLength = brush.lineRenderers.Count;

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
