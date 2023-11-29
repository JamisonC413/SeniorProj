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
    public int oldLineListLength = 0;
    public int oldMeshListLength = 0;

    public int axis;

    // Start is called before the first frame update
    void Start()
    {
        brush = GameObject.FindGameObjectWithTag("brush").GetComponent<Brush>();
    }

    public override void execute()
    {
        //Sets initial values, mirror logic is handled in mirrorBlockBottom

        oldLineListLength = brush.lineRenderers.Count;
        oldMeshListLength = brush.meshRenderers.Count;
    }
}
