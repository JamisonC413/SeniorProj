using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class duplicateBlock : NestedBlock
{
    //Text field for number of duplicates
    [SerializeField]
    private TMP_InputField numDuplicate;

    //The brush used to get line/mesh renderer lists
    public Brush brush;

    //Initial renderer list lengths
    private int oldLineListLength = 0;
    private int oldMeshListLength = 0;

    //Flag to check if the duplicate block is currently running
    public bool flag;
    //Number of duplicates to make
    public int duplicate;

    //Start position of the brush to rotate duplicates around
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        brush = GameObject.FindGameObjectWithTag("brush").GetComponent<Brush>();
        flag = false;
    }

    public override void execute()
    {
        //Read in the number of duplicates
        string inputData = numDuplicate.text;
        if (!string.IsNullOrEmpty(inputData) && int.TryParse(inputData, out int parsed))
        {
            duplicate = parsed;
        }
        else
        {
            //Default to one duplicate if no text is input
            duplicate = 1;
        }
        //Set the flag to true to indicate block is running and update initial values
        flag = true;
        oldLineListLength = brush.lineRenderers.Count;
        oldMeshListLength = brush.meshRenderers.Count;
        startPos = brush.transform.position;
    }

    // Update is called once per frame
    public override void updateExecute()
    {
        //Compute the angle size according to how many duplicates you want
        int angleSize = 360 / (duplicate + 1);

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
                //Make # of duplicates specified
                for (int i = 0; i < duplicate; i++)
                {
                    //Make a copy of the linerenderer and rotate it around the starting brush position on the z axis
                    GameObject rotate = Instantiate(brush.lineRenderers[newLineLength - 1], new Vector3(0, 0, 0), Quaternion.identity);
                    rotate.GetComponent<LineRenderer>().transform.RotateAround(startPos, Vector3.forward, angleSize*(i+1));

                    Renderer rendererComponent = rotate.GetComponent<Renderer>();
                    if (rendererComponent != null)
                    {
                        rendererComponent.sortingLayerName = "ImageRendering";
                        rendererComponent.sortingOrder = 1;
                    }

                    //Add this new line renderer to the list and increase the list count
                    brush.lineRenderers.Add(rotate);
                    oldLineListLength = brush.lineRenderers.Count;
                }
               
            }

            if (newMeshLength > oldMeshListLength)
            {
                for (int i = 0; i < duplicate; i++)
                {
                    //Make a copy of the meshrenderer and rotate it around the starting brush position on the z axis
                    GameObject rotate = Instantiate(brush.meshRenderers[newMeshLength - 1], brush.meshRenderers[newMeshLength - 1].transform.position, Quaternion.identity);
                    rotate.GetComponent<MeshRenderer>().transform.RotateAround(startPos, Vector3.forward, angleSize * (i + 1));

                    Renderer rendererComponent = rotate.GetComponent<Renderer>();
                    if (rendererComponent != null)
                    {
                        rendererComponent.sortingLayerName = "ImageRendering";
                        rendererComponent.sortingOrder = 1;
                    }
                    //Add this new mesh renderer to the list and increase the list count
                    brush.meshRenderers.Add(rotate);
                    oldMeshListLength = brush.meshRenderers.Count;
                }
            }
        }
    }
}
