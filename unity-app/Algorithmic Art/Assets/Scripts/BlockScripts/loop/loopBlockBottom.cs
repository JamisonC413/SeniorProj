using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loopBlockBottom : nestedBottom
{
    private Block tempNextBlock = null;

    private bool firstRun = true;
    public override void updateExecute()
    {
        //(loopBlock)topBlock.nextBlock
    }

    public override void execute()
    {
        if(firstRun)
        {
            tempNextBlock = nextBlock;
            firstRun = false;
        }

        if (((loopBlock)topBlock).repeat >= 1)
        {
            nextBlock = topBlock;
        }
        else
        {
            nextBlock = tempNextBlock;
            tempNextBlock = null;
            firstRun = true;
        }
    }
}
