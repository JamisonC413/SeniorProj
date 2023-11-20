using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class NestedBlock : Block
{
    // Used as a label for block mover to acknowledge the script correctly.
    // Nested blocks are always picked up and moved with moveChildren so that contents remain inside properly.
    [SerializeField]
    private GameObject snap1;

    [SerializeField]
    private GameObject snap2;

    public nestedBottom bottomBlock;

    [SerializeField]
    private GameObject bottomBlockPrefab;

    [SerializeField]
    private GameObject backGround;

    public Play playScript;

    // Start is called before the first frame update
    void Awake()
    {
        snapPositions = new Vector2[2];
        snapPositions[0] = snap1.transform.position;
        snapPositions[1] = snap2.transform.position;

        initialize();

        playScript = GameObject.Find("PlayHandler").GetComponent<Play>();
    }

    // Update is called once per frame


    public void initialize()
    {
        this.blockID = Block.nextID;
        this.prevBlock = null;
        this.nextBlock = null;

        Block.nextID++;
    }

    void Update()
    {
        // Updates the snap positions with any new position of block
        snapPositions[0] = snap1.transform.position;
        snapPositions[1] = snap2.transform.position;

        if (!bottomBlock)
        {
            // Create the prefab 
            GameObject spawnedObject2 = Instantiate(bottomBlockPrefab, gameObject.transform.position, Quaternion.identity);
            //Vector3 scale = new Vector3(33, 28f, 0);

            spawnedObject2.transform.localScale = gameObject.transform.localScale;

            bottomBlock = spawnedObject2.GetComponent<nestedBottom>();
            bottomBlock.topBlock = this;
            this.nextBlock = bottomBlock;
            bottomBlock.prevBlock = this;

            Renderer renderer = spawnedObject2.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sortingLayerName = "Block";
                renderer.sortingOrder = 0;
            }
            else
            {
                Debug.Log("There must be a renderer component to the spawned prefab");
            }
            spawnedObject2.tag = "block";
            // Was having difficulty lining up the top newly spawned bottom block on bottom of this block's snapPositions[1]. Hard coded for now.  
            spawnedObject2.transform.position = (Vector2)this.snapPositions[1] - new Vector2(0.05f, .1f);
            //Debug.Log((spawnedObject2.GetComponent<Block>()).snapPositions[0]);
            //Debug.Log((Vector2)this.snapPositions[1]);

        }
        else
        {
            backGround.transform.localScale = new Vector3(backGround.transform.localScale.x, (gameObject.transform.position.y - bottomBlock.transform.position.y) * (1 / gameObject.transform.localScale.y), 0f);
            backGround.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - ((gameObject.transform.position.y - bottomBlock.transform.position.y) / 2), gameObject.transform.position.z);
        }

    }


    //public override Block getNextPlayBlock()
    //{
    //    return bottomBlock.nextBlock;
    //}
}
