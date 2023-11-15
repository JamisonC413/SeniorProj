using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loopBlock : NestedBlock
{

    [SerializeField]
    private GameObject snap1;

    [SerializeField]
    private GameObject snap2;

    public loopBlockBottom bottomBlock;

    [SerializeField]
    private GameObject bottomBlockPrefab;

    public Play playScript;

    [SerializeField]
    private GameObject sideBar;
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

        if (!bottomBlock)
        {
            // Create the prefab 
            GameObject spawnedObject2 = Instantiate(bottomBlockPrefab, gameObject.transform.position, Quaternion.identity);
            //Vector3 scale = new Vector3(33, 28f, 0);

            spawnedObject2.transform.localScale = gameObject.transform.localScale;
            Debug.Log(gameObject.transform.localScale);

            bottomBlock = spawnedObject2.GetComponent<loopBlockBottom>();
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
            spawnedObject2.transform.position = this.snapPositions[1];//- (spawnedObject2.GetComponent<Block>()).snapPositions[0]);
        }

    }

    public void initialize()
    {
        this.blockID = Block.nextID;
        this.prevBlock = null;
        this.nextBlock = null;

        Block.nextID++;
    }

    public override void execute()
    {
        StartCoroutine(loopExecute());
    }

    private IEnumerator loopExecute()
    {
        Block block = this.nextBlock;
        while (block != null)
        {
            Debug.Log(block + "was run");
            block.gameObject.GetComponent<SpriteRenderer>().sprite = block.selected;
            block.execute();
            yield return new WaitForSeconds(playScript.delay);
            block.gameObject.GetComponent<SpriteRenderer>().sprite = block.defaultSprite;
            block = block.nextBlock;
        }
    }

}
