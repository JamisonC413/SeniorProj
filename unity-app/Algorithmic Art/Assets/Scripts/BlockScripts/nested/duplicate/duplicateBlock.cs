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
    public int oldLineListLength = 0;
    public int oldMeshListLength = 0;

    //Number of duplicates to make
    public int duplicate;

    //Start position of the brush to rotate duplicates around
    public Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        brush = GameObject.FindGameObjectWithTag("brush").GetComponent<Brush>();
    }

    public override void execute()
    {
        //Sets initial values, duplication logic is handled in duplicateBlockBottom

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
        oldLineListLength = brush.lineRenderers.Count;
        oldMeshListLength = brush.meshRenderers.Count;
        startPos = brush.transform.position;
    }

}
