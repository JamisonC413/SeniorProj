using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class duplicateBlock : NestedBlock
{
    public Brush brush;

    private int oldLineListLength = 0;
    private int oldMeshListLength = 0;


    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        brush = GameObject.FindGameObjectWithTag("brush").GetComponent<Brush>();
        int oldLineListLength = brush.lineRenderers.Count;
        int oldMeshListLength = brush.meshRenderers.Count;
        startPos = brush.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        int newLength = brush.lineRenderers.Count;
        if (newLength > oldLineListLength)
        {
            GameObject rotate = brush.lineRenderers[newLength - 1];
            rotate.GetComponent<LineRenderer>().transform.RotateAround(startPos, Vector3.right, 180);
        }
        if (newLength > oldMeshListLength)
        {
            GameObject rotate = brush.meshRenderers[newLength - 1];
            //rotate.rot
        }
        
    }
}
