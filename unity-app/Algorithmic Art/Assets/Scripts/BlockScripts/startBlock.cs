using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startBlock : Block
{
    // Offset from center for the snap point
    [SerializeField]
    private Vector2 snapOffset = new Vector2(0f, 1f);

    // Called once when the code starts 
    void Awake()
    {
        // Sets ID, refrences and boolean for snapped
        this.blockID = Block.nextID;
        this.topSnapped = false;
        this.botSnapped = false;
        this.prevBlock = null;
        this.nextBlock = null;

        // Increments nextID for next block
        Block.nextID++;

        // Creates snapPositions
        // Note: Should probably paramaterize the offset but it shouldn't change so I see it as a prefrence thing
        snapPositions = new Vector2[1];
        snapPositions[0] = new Vector2(transform.position.x, transform.position.y) + snapOffset;
        Debug.Log(snapPositions[0]);
    }

    // Update just set up to update the snapPositions of startBlock
    private void Update()
    {
        snapPositions[0] = new Vector2(transform.position.x, transform.position.y) + snapOffset;
    }
}
