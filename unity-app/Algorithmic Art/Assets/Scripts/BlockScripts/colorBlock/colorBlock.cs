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
    private Play[] playScripts;

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

        playScripts = new Play[2];
        playScripts[0] = GameObject.Find("Play").GetComponent<Play>();
        playScripts[1] = GameObject.Find("FastForward").GetComponent<Play>();
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
<<<<<<< HEAD
        playScripts[0].currentColor = (Play.Color)color;
        playScripts[1].currentColor = (Play.Color)color;
=======
        switch (color)
        {
            case 0:
                playScripts[0].currentColor = Color.red;
                playScripts[1].currentColor = Color.red;
                break;
            case 1:
                playScripts[0].currentColor = Color.green;
                playScripts[1].currentColor = Color.green;

                break;
            case 2:
                playScripts[0].currentColor = Color.blue;
                playScripts[1].currentColor = Color.blue;
                break;
            case 3:
                playScripts[0].currentColor = Color.black;
                playScripts[1].currentColor = Color.black;
                break;
            default: break;
        }

>>>>>>> dbf3251a0995ebd93f509699c1af083e98d7b0cb

    }
}