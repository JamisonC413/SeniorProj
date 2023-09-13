using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // Static variable to keep track of the next available ID
    public static int nextID = 0;

    public int blockID;
    public Color blockColor;
}
