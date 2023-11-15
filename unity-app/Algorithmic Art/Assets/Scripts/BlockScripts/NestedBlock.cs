using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestedBlock : Block
{
    // Used as a label for block mover to acknowledge the script correctly.
    // Nested blocks are always picked up and moved with moveChildren so that contents remain inside properly.
}
