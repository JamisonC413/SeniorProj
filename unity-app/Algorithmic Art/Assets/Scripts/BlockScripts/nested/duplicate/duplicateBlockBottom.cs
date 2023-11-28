using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class duplicateBlockBottom : nestedBottom
{ 
    public override void execute()
    {
        ((duplicateBlock)topBlock).flag = false;
    }
}
