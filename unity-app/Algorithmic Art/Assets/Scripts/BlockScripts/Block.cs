using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Block outlines all functionality that is expected of blocks.
public class Block : MonoBehaviour
{
    // Static variable to keep track of the next available ID
    public static int nextID = 0;

    // Keeps track of the render layer
    public static int layer = 0;

    // Unique block ID 
    public int blockID;

    // Prototype for color tracking NOT IMPLEMENTED
    public Color blockColor;

    // Tracks if a block snapped above
    public bool topSnapped;

    // Tracks if a block snapped below
    public bool botSnapped;

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
        Vector3 newPosition = new Vector3(translation.x, translation.y, transform.position.z);

        // Updates position
        transform.position = newPosition;

        // Next block is updated with the same vector. These are all the children of this block
        if (nextBlock != null)
        {
            nextBlock.moveChildren(translation);
        }
    }

    // Execute function used by every block. In the block's specific script this will be overriden by each block
    virtual public void execute()
    {
        //yield return null;
    }
}
