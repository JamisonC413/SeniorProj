using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loopBlockBottom : Block
{
    [SerializeField]
    private GameObject snap1;

    [SerializeField]
    private GameObject snap2;

    public loopBlock topBlock;

    // Start is called before the first frame update
    void Awake()
    {
        snapPositions = new Vector2[2];
        snapPositions[0] = snap1.transform.position;
        snapPositions[1] = snap2.transform.position;

        initialize();
    }

    // Update is called once per frame
    void Update()
    {
        // Updates the snap positions with any new position of block
        snapPositions[0] = snap1.transform.position;
        snapPositions[1] = snap2.transform.position;

        if (!prevBlock && topBlock)
        {
            prevBlock = topBlock;
            topBlock.nextBlock = this;
        }
    }

    public void initialize()
    {
        this.blockID = Block.nextID;
        this.prevBlock = null;
        this.nextBlock = null;

        Block.nextID++;
    }
}
