using Codice.Client.Common.Locks;
using System;
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

    public Brush brush2;

    // Delay between blocks playing (seconds)
    public float delay = 1f;


    public Color currentColor = Color.black;

    public float lineWidth = .03f;

    public float lineWidth2 = .055f;

    public bool locked = false;

    void Start()
    {
        
    }

    // Handles rendering. Is an IEnumerator so that it can be paused
    public IEnumerator Render()
    {

        // Resets brush to it's origin
        brush.resetPosition();
        brush.clearRenderers();
        brush2.resetPosition();
        brush2.clearRenderers();
        currentColor = Color.black;
        lineWidth = 0.03f;
        lineWidth2 = .055f;


        yield return new WaitForSeconds(delay);


        // Tracks the block that we are currently on, allowing us to iterate through the block list
        Block block = startScript.nextBlock;
        while (block != null)
        {
            Debug.Log(block + "was run");
            block.gameObject.GetComponent<SpriteRenderer>().sprite = block.selected;
            block.execute();
            yield return new WaitForSeconds(delay);
            block.gameObject.GetComponent<SpriteRenderer>().sprite = block.defaultSprite;
            block = block.nextBlock;
        }
        locked = false;
        yield return null;
    }

    //Starts the rendering coroutine
    public void StartRendering()
    {
        if (!locked)
        {
            locked = true;
            StartCoroutine(Render());
        }
    }
}
