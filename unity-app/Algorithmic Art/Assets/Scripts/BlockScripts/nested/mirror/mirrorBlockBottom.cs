using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mirrorBlockBottom : nestedBottom
{ 
    public override void execute()
    {
        ((mirrorBlock)topBlock).flag = false;
        ((mirrorBlock)topBlock).brush.numMirrors--;
    }
}
