using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawBlock : Block
{

    void Awake()
    {
        this.blockID = Block.nextID;
        this.topSnapped = false;
        this.botSnapped = false;
        this.prev = null;
        this.next = null;

        Block.nextID++;

        snapPositions = new Vector2[2];
        snapPositions[0] = new Vector2(transform.position.x, transform.position.y - .6f);
        snapPositions[1] = new Vector2(transform.position.x, transform.position.y + .6f);

        //Debug.Log(snapPositions[0]);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        snapPositions[0] = new Vector2(transform.position.x, transform.position.y - .6f);
        snapPositions[1] = new Vector2(transform.position.x, transform.position.y + .6f);
    }
}
