using Codice.CM.Client.Differences;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class functionStart : Block
{
    public string startName;

    // Top snap point
    [SerializeField]
    private GameObject snap1;


    public Play playScript;
    void Awake()
    {
        this.blockID = Block.nextID;
        this.prevBlock = null;
        this.nextBlock = null;

        Block.nextID++;
        startName = null;

        snapPositions = new Vector2[2];
        snapPositions[0] = snap1.transform.position;

        playScript = GameObject.FindGameObjectWithTag("playHandler").GetComponent<Play>();
    }

    void Update()
    {
        // Updates the snap positions with any new position of block
        snapPositions[0] = snap1.transform.position;
    }

    public void OnDestroy()
    {

    }
}
