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
    private GameObject snap1;

    [SerializeField]
    private GameObject snap2;

    [SerializeField]
    private Play playScript;

    public int color = 0;

    // Sets the starting information for the block, ID, refrences and snap positions
    void Awake()
    {
        this.blockID = Block.nextID;
        this.prevBlock = null;
        this.nextBlock = null;

        Block.nextID++;

        snapPositions = new Vector2[2];
        snapPositions[0] = snap1.transform.position;
        snapPositions[1] = snap2.transform.position;

        playScript = GameObject.FindGameObjectWithTag("playHandler").GetComponent<Play>();
    }

    // Used to update information on the draw block
    void Update()
    {
        // Updates the snap positions with any new position of block
        snapPositions[0] = snap1.transform.position;
        snapPositions[1] = snap2.transform.position;

        // Checks the x and Y input for valid integers, if non found than sets to default value 1

    }

    // Will be used to draw line using a child linerenderer component. Not yet implemented
    public override void execute()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = selected;
        switch (color)
        {
            case 0:
                playScript.currentColor = Color.red;
                break;
            case 1:
                playScript.currentColor = Color.green;
                break;
            case 2:
                playScript.currentColor = Color.blue;
                break;
            case 3:
                playScript.currentColor = Color.black;
                break;
            default: break;
        }
    }
}