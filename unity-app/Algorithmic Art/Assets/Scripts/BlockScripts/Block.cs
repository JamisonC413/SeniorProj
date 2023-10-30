using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

// Block outlines all functionality that is expected of blocks.
public class Block : MonoBehaviour
{
    // Static variable to keep track of the next available ID
    public static int nextID = 0;

    // Keeps track of the render layer
    public static int layer = 0;

    // Unique block ID    NOTE: BlockID is not currently used to check snap points (remove?)
    public int blockID;

    // Prototype for color tracking NOT IMPLEMENTED
    public Color blockColor;

    // Refrence to block above
    public Block prevBlock;

    // Refrence to block below
    public Block nextBlock;

    // Tracks all positions that can be used to snap to this block
    public Vector2[] snapPositions;

    // Prototype for moving all children blocks of this block. This updates all the transforms to the same as the translation param
    public void moveChildren(Vector2 translation)
    {
        // newPosition is the new position of the gameObject (only effects x and y)
        Vector3 newPosition = new Vector3(translation.x + gameObject.transform.position.x, translation.y + gameObject.transform.position.y, transform.position.z);

        // Updates position
        transform.position = newPosition;

        // Next block is updated with the same vector. These are all the children of this block
        if (nextBlock != null)
        {
            nextBlock.moveChildren(translation);
        }
    }
    public float getListHeight()
    {
        float hieght = snapPositions[0].y - snapPositions[1].y;
        // Next block is updated with the same vector. These are all the children of this block
        if (nextBlock != null)
        {
            return hieght + nextBlock.getListHeight();
        }
        return hieght;
    }
    public Block getLastBlock()
    {
        if (nextBlock != null)
        {
            return nextBlock.getLastBlock();
        }
        return this;
    }

    // Execute function used by every block. In the block's specific script this will be overriden by each block
    virtual public void execute()
    {
        //yield return null;
    }

    public void setRenderLayersHigh()
    {
        // Get the Canvas component from the block
        Canvas canvasComponent = gameObject.GetComponentInChildren<Canvas>();

        // Gets the rendering component of the block and sets its layer to be above all UI 
        Renderer rendererComponent = gameObject.GetComponent<Renderer>();
        if (rendererComponent != null)
        {
            rendererComponent.sortingLayerName = "Block";
        }

        // Check if a Canvas component is found
        if (canvasComponent != null)
        {
            // Set the sorting layer of the Canvas component
            canvasComponent.sortingLayerName = "Block";
            // Set the sorting order of the Canvas component
            canvasComponent.sortingOrder = 1;
        }

        if (nextBlock != null)
        {
            nextBlock.setRenderLayersHigh();
        }
    }

    public void setRenderLayersLow()
    {
        Renderer rendererComponent = gameObject.GetComponent<Renderer>();

        Canvas canvasComponent = gameObject.GetComponentInChildren<Canvas>();

        // Sets the blocks' rendering layer back to a normal blocks so that it appears under UI, palette and canvas
        rendererComponent.sortingLayerName = "Block Background";
        rendererComponent.sortingOrder = 1;

        // Set the sorting layer of the Canvas component
        canvasComponent.sortingLayerName = "Block Background";
        // Set the sorting order of the Canvas component
        canvasComponent.sortingOrder = 2;

        if (nextBlock != null)
        {
            nextBlock.setRenderLayersLow();
        }
    }
}