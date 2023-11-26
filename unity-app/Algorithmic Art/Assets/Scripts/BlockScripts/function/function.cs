using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Script for the Draw Block
public class function : Block
{
    // Stores the gamobject used to track top snap position
    [SerializeField]
    private GameObject snap1;

    // Bottom snapObject
    [SerializeField]
    private GameObject snap2;

    // The brushes gameobject 
    public Play playScript;

    private bool firstRun;

    [SerializeField]
    private string nameKey;

    private Block tempNextBlock;

    private Block tempLastBlock;
    // Sets the starting information for the block, ID, refrences and snap positions
    void Awake()
    {
        initialize();

        snapPositions = new Vector2[2];
        snapPositions[0] = snap1.transform.position;
        snapPositions[1] = snap2.transform.position;

        //Note: Replaced GameObject.Find with GameObject.FindGameObjectWithTag because Find is very expensive - Tong
        playScript = GameObject.FindGameObjectWithTag("playHandler").GetComponent<Play>();
        firstRun = true;
        tempNextBlock = null;
        tempLastBlock = null;
    }

    // Used to update information on the draw block
    void Update()
    {
        snapPositions[0] = snap1.transform.position;
        snapPositions[1] = snap2.transform.position;
    }

    public void initialize()
    {
        this.blockID = Block.nextID;
        this.prevBlock = null;
        this.nextBlock = null;

        Block.nextID++;
    }

    // Will be used to draw line using a child linerenderer component. Not yet implemented
    public override void execute()
    {
        if(firstRun && playScript.functions.ContainsKey(nameKey))
        {
            functionStart startScript = playScript.functions[nameKey];

            tempLastBlock = startScript.getLastBlock();
            tempLastBlock.nextBlock = this;
            tempNextBlock = this.nextBlock;

            this.nextBlock = startScript;
            firstRun = false;
        }
        else if(!firstRun)
        {
            if (tempNextBlock != null)
            {
                tempLastBlock.nextBlock = null;
                tempLastBlock = null;
            }
            this.nextBlock = tempNextBlock;
            tempNextBlock = null;
            firstRun = true;
        }

    }
}