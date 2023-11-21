using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loopBlockBottom : nestedBottom
{
    private Block tempNextBlock = null;
    public override void updateExecute()
    {
        //(loopBlock)topBlock.nextBlock
    }

    public override void execute()
    {
        if(!tempNextBlock)
        {
            tempNextBlock = nextBlock;
        }

        if (((loopBlock)topBlock).repeat > 0)
        {
            nextBlock = topBlock;
        }
        else
        {
            nextBlock = tempNextBlock;
            tempNextBlock = null;
        }
    }
}
