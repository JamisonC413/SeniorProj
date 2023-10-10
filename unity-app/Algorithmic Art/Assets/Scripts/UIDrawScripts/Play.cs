using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

// Handles drawing the lines using the play button
public class Play : MonoBehaviour
{
    // LineRenderer component to use
    // Note: new system will have draw blocks deal with these 
    public LineRenderer lineRenderer;

    // The script for the startBlock
    public startBlock startScript;

    // The brushes gameobject
    public Brush brush;

    // Delay between blocks playing (seconds)
    public float delay = 1f;
  


    // Handles rendering. Is an IEnumerator so that it can be paused
    public IEnumerator Render()
    {

        // Resets brush to it's origin
        brush.resetPosition();

        // Tracks the block that we are currently on, allowing us to iterate through the block list
        Block block = startScript.nextBlock;

        block.execute(delay);
        yield return null;
        //// Gets the x and y inputs to the block
        //float blockX = ((drawBlock)block).X;
        //float blockY = ((drawBlock)block).Y;
        //// transform is equal to the last x or y + the blockX
        //float xTransform = lastx + blockX;
        //float yTransform = lasty + blockY;

        //// Create bounds for the lines, currently only the bottom and left are bounded
        //if (xTransform < 0)
        //{
        //    xTransform = 0;
        //}
        //if (yTransform < 0)
        //{
        //    yTransform = 0;
        //}
        //// Misc Debug
        //Debug.Log(brush.transform.position);
        //Debug.Log(new Vector3(xTransform, yTransform, 0f));

        //// Add the point from the block to the line renderer
        //positions.Add(new Vector3(xTransform, yTransform, 0f));
        //// Move to next block
        //block = block.nextBlock;

        //// Render lines
        //lineRenderer.positionCount = positions.Count;
        //lineRenderer.SetPositions(positions.ToArray());

        //// Move brush
        //brush.transform.position = startBrush + new Vector3(xTransform, yTransform, 0f);

        //// Update last x and y
        //lasty = yTransform;
        //lastx = xTransform;

        //yield return new WaitForSeconds(delay);

    }

    //Starts the rendering coroutine
    public void StartRendering()
    {
        StartCoroutine(Render());
    }
}
