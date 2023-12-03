using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    public Color currentColor = Color.black;

    public float lineWidth = .03f;

    public float lineWidth2 = .055f;

    public bool locked = false;

    public Block currentBlock;

    public Dictionary<string, functionStart> functions = new Dictionary<string, functionStart>();


    // Handles rendering. Is an IEnumerator so that it can be paused
    public IEnumerator Render()
    {

        // Resets brush to it's origin
        brush.resetPosition();
        brush.clearRenderers();
        currentColor = Color.black;
        lineWidth = 0.03f;
        lineWidth2 = .055f;


        yield return new WaitForSeconds(delay);


        // Tracks the block that we are currently on, allowing us to iterate through the block list
        currentBlock = startScript.nextBlock;
        while (currentBlock != null)
        {
            Debug.Log(currentBlock + "was run");
            currentBlock.gameObject.GetComponent<SpriteRenderer>().sprite = currentBlock.selected;

            currentBlock.execute();

            yield return new WaitForSeconds(delay);
        
            currentBlock.gameObject.GetComponent<SpriteRenderer>().sprite = currentBlock.defaultSprite;
            currentBlock = currentBlock.nextBlock;
        }
        locked = false;
        yield return null;
    }

    //Starts the rendering coroutine
    public void StartRendering(float delay)
    {
        this.delay = delay;
        if (!locked)
        {
            locked = true;
            StartCoroutine("Render");
        }
    }

    public void StopRendering()
    {
        if (locked)
        {
            StopCoroutine("Render");
            if (currentBlock != null)
            {
                currentBlock.gameObject.GetComponent<SpriteRenderer>().sprite = currentBlock.defaultSprite;
            }

            locked = false;
        }
    }
}
