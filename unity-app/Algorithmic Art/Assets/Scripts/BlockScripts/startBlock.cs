using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startBlock : Block
{
    // Gameobject for the snap point
    [SerializeField]
    public GameObject snapPosition;

    // Called once when the code starts, simple setting of variables
    void Awake()
    {
        // Sets ID and refrences for initial setup
        this.blockID = Block.nextID;
        this.prevBlock = null;
        this.nextBlock = null;

        // Increments nextID for next block
        Block.nextID++;

        // Creates snapPosition, startBlock only has one which complicates some code in the blockmover. All other blocks
        // are garunteed to have 2 snapPositions
        snapPositions = new Vector2[1];
        snapPositions[0] = snapPosition.transform.position;
    }

    // Update just set up to update the snapPositions of startBlock, potentially change so it only updates when moved? 
    // Currently have no idea how to do that so this'll do for now.
    private void Update()
    {
        snapPositions[0] = snapPosition.transform.position;
    }
}