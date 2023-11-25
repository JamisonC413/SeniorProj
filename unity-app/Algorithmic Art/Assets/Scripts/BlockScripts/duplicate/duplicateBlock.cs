using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class duplicateBlock : NestedBlock
{
    public Brush brush;

    private int oldLineListLength = 0;
    private int oldMeshListLength = 0;

    public bool flag;

    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        brush = GameObject.FindGameObjectWithTag("brush").GetComponent<Brush>();
        flag = false;
    }

    public override void execute()
    {
        flag = true;
        oldLineListLength = brush.lineRenderers.Count;
        oldMeshListLength = brush.meshRenderers.Count;
        startPos = brush.transform.position;
    }

    // Update is called once per frame
    public override void updateExecute()
    {
        if (flag)
        {
          //  Debug.Log("OldLength:" + oldLineListLength);
            int newLength = brush.lineRenderers.Count;
            int newMeshLength = brush.meshRenderers.Count;
            if (newLength > oldLineListLength)
            {
                Debug.Log("Rotating");
                GameObject rotate = Instantiate(brush.lineRenderers[newLength - 1], brush.startPosition, Quaternion.identity);
                rotate.GetComponent<LineRenderer>().transform.RotateAround(startPos, Vector3.fwd, 180);
                brush.lineRenderers.Add(rotate);
                oldLineListLength = brush.lineRenderers.Count;
            }
            if (newMeshLength > oldMeshListLength)
            {
                GameObject rotate = brush.meshRenderers[newLength - 1];
                //rotate.rot
            }
        }
    }
}
