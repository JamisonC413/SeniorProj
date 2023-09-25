using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startBlock : Block
{

    public GameObject next;

    // Start is called before the first frame update
    //void Start()
    //{
    //    //Debug.Log(gameObject.layer);

    //    // Debug.Log(snapPosition);
    //}

    // Update is called once per frame
    void Awake()
    {
        this.blockID = Block.nextID;
        this.topSnapped = false;
        this.botSnapped = false;
        this.connectedBlocks = new Block[1];

        Block.nextID++;

        snapPositions = new Vector2[1];
        snapPositions[0] = new Vector2(transform.position.x, transform.position.y) + new Vector2(0.0f, -.8f);
        Debug.Log(snapPositions[0]);
    }
}
