using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

// Handles drawing the lines using the play button
public class PlayButton : MonoBehaviour
{
    public Play playScript;

    public int delay;

    //Starts the rendering coroutine
    public void StartRendering()
    {
        playScript.delay = delay;
        playScript.StartRendering();
    }
}
