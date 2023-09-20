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
    public Block[] connectedBlocks;
}
