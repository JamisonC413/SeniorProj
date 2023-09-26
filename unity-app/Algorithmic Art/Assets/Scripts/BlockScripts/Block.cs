using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // Static variable to keep track of the next available ID
    public static int nextID = 0;
    public static int layer = 0;

    public int blockID;
    public Color blockColor;
    public bool topSnapped;
    public bool botSnapped;
    public Block prevBlock;
    public Block nextBlock;
    public Vector2[] snapPositions;

    public void moveChildren(Vector2 translation)
    {
        Vector3 newPosition = new Vector3(translation.x, translation.y, transform.position.z);
        transform.position = newPosition;

        if (nextBlock != null)
        {
            nextBlock.moveChildren(translation);
        }
    }
   
    public virtual void execute()
    {
        // Do nothing
    }
}
