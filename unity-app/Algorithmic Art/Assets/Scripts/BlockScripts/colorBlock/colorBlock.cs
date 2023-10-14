using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Script for the Draw Block
public class colorBlock : Block
{

    // Offset for snaps above and below
    [SerializeField]
    private Vector2 snapOffset = new Vector2(0f, 1f);

    // Default for x and y coordinates
    [SerializeField]
    private int defaultOption = 0;

    [SerializeField]
    private Play[] playScripts;

    public int color = 0;

    // Sets the starting information for the block, ID, refrences and snap positions
    void Awake()
    {
        this.blockID = Block.nextID;
        this.topSnapped = false;
        this.botSnapped = false;
        this.prevBlock = null;
        this.nextBlock = null;

        Block.nextID++;

        snapPositions = new Vector2[2];
        snapPositions[0] = new Vector2(transform.position.x, transform.position.y) + snapOffset;
        snapPositions[1] = new Vector2(transform.position.x + snapOffset.x, transform.position.y - snapOffset.y);

        playScripts = new Play[2];
        playScripts[0] = GameObject.Find("Play").GetComponent<Play>();
        playScripts[1] = GameObject.Find("FastForward").GetComponent<Play>();
    }

    // Used to update information on the draw block
    void Update()
    {
        // Updates the snap positions with any new position of block
        snapPositions[0] = new Vector2(transform.position.x, transform.position.y) + snapOffset;
        snapPositions[1] = new Vector2(transform.position.x + snapOffset.x, transform.position.y - snapOffset.y);

        // Checks the x and Y input for valid integers, if non found than sets to default value 1

    }

    // Will be used to draw line using a child linerenderer component. Not yet implemented
    public override void execute()
    {
        playScripts[0].currentColor = (Play.Color) color;
        playScripts[1].currentColor = (Play.Color) color;

    }
}
