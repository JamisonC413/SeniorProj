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

    // Default sprite is the sprite used for the block when coding
    public Sprite defaultSprite;

    // Selected is the highlighted version of the block sprite, switches to the highlighted version when block is playing
    public Sprite selected;

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

    // Gets the height of the list of blocks (distance from highest snap position to lowest)
    public float getListHeight()
    {
        // Difference
        float hieght = snapPositions[0].y - snapPositions[1].y;

        // Recursive call to children if they exist, adding to the overall height
        if (nextBlock != null)
        {
            return hieght + nextBlock.getListHeight();
        }
        return hieght;
    }
    // Gets the last block in the list of blocks
    public Block getLastBlock()
    {
        // Simple recursive call to next block, traversing list
        if (nextBlock != null)
        {
            return nextBlock.getLastBlock();
        }
        return this;
    }

    // Execute function used by every block. In the block's specific script this will be overriden by each block
    virtual public IEnumerator execute()
    {
        yield return null;
    }

    // Sets the block and any canvas child it has to render above other UI and game elements 
    // NOTE: As of updating the system to only allow scrolling this functionality can likely be simplified. Probably don't need
    // to change the render layer since they cannot be panned over the left or right windows.
    public void setRenderLayersHigh()
    {
        // Get the Canvas component from the block
        Canvas canvasComponent = gameObject.GetComponentInChildren<Canvas>();

        // Gets the rendering component of the block and sets its layer to be above all UI 
        Renderer rendererComponent = gameObject.GetComponent<Renderer>();
        if (rendererComponent != null)
        {
            rendererComponent.sortingLayerName = "Block";
            rendererComponent.sortingOrder = 1;
        }

        // Check if a Canvas component is found
        if (canvasComponent != null)
        {
            // Set the sorting layer of the Canvas component
            canvasComponent.sortingLayerName = "Block";
            // Set the sorting order of the Canvas component
            canvasComponent.sortingOrder = 2;
        }

        // Call on all children
        if (nextBlock != null)
        {
            nextBlock.setRenderLayersHigh();
        }
    }

    // Sets the render layers of block to the proper value 
    public void setRenderLayersLow()
    {
        // Get the renderer component and canvas component
        Renderer rendererComponent = gameObject.GetComponent<Renderer>();
        Canvas canvasComponent = gameObject.GetComponentInChildren<Canvas>();

        // If renderer and canvas exist than they will be set to the proper values.
        if (rendererComponent != null)
        {
            // Sets the blocks' rendering layer back to a normal blocks so that it appears under UI, palette and canvas
            rendererComponent.sortingLayerName = "Block Background";
            rendererComponent.sortingOrder = 1;
        }

        if (canvasComponent != null)
        {
            // Set the sorting layer of the Canvas component
            canvasComponent.sortingLayerName = "Block Background";
            // Set the sorting order of the Canvas component
            canvasComponent.sortingOrder = 2;
        }

        // Do for all children
        if (nextBlock != null)
        {
            nextBlock.setRenderLayersLow();
        }
    }

    virtual public Block getNextPlayBlock()
    {
        // Not defined by default
        return nextBlock;
    }
}