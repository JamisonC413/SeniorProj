using Codice.CM.Client.Differences;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class functionStart : Block
{
    public string name;

    public Play playScript;
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

    public void OnDestroy()
    {
    }
}
