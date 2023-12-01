using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loopBlockBottom : nestedBottom
{
    private Block tempNextBlock = null;

    private bool firstRun = true;

    public Play play;

    public override void updateExecute()
    {
        if (play && !play.locked)
        {
            nextBlock = tempNextBlock;
            tempNextBlock = null;
            firstRun = true;
        }
    }

    public override void execute()
    {
        play = GameObject.FindGameObjectWithTag("playHandler").GetComponent<Play>();

        if (firstRun)
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
