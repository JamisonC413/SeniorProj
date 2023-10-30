using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;
using System.Threading;

// Handles moving and snapping blocks.
public class blockMover : MonoBehaviour
{
    // Offset tracks the difference between the mouse position and the transform of the block
    private Vector3 offset;

    // Tracks if a block is getting dragged
    private bool isDragging = false;

    // The search radius for finding blocks to snap to
    public float searchRadius = .1f;

    [SerializeField]
    private GameObject startBlock;

    // The block currently getting dragged
    // NOTE: expirimented with removing isDragging and replacing it with a null check on this was done... but it didn't work
    //       maybe someone else will have a better idea?
    public GameObject block;

    // Tracks if a block is getting inserted into the middle of a list
    public Block insertingBlock = null;

    private bool insertingBlockChanged = false;

    private Vector2[] oldSnapPositions = null;

    public Block newPrevBlock = null;

    public Block newNextBlock = null;

    public Block oldPrevBlock = null;

    public Block oldNextBlock = null;

    private bool dragChildren = false;

    private Block blockScript = null;

    private Vector2 lastMousePos = Vector2.zero;

    public GameObject parentCanvas;

    // Update is called every frame and handles dragging and snapping
    void Update()
    {
        // Split into two main sections, button down to initiate dragging and a isDragging section

        // When button is pressed checks if there is a block underneath and then grabs it
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the mouse position and grab the first object hit
            Ray ray = new Ray();

            if (Camera.main != null && Camera.main.isActiveAndEnabled)
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                
            }
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            // Helpful debug code, tells you what object gets hit by raycast
            //if (hit.collider != null)
            //{
            //    Debug.Log(hit.collider.gameObject);
            //}

            // If the raycast collides with object than check the tag for block
            if (hit.collider != null && hit.collider.CompareTag("block"))
            {
                // Store the block reference as the block we are dragging and calculate the offset
                block = hit.collider.gameObject;
                blockScript = (Block)block.GetComponent("Block");
                block.transform.SetParent(parentCanvas.transform);
                offset = block.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                

                // Set isDragging to true
                isDragging = true;

                if (((Block)block.GetComponent("Block")).prevBlock)
                {
                    dragChildren = false;
                }
                else
                {
                    dragChildren = true;
                }
                // Sets refrences of the block to null, also sets the refrences of blocks being broken away from
                setRefrences();
                blockScript.setRenderLayersHigh();
            }
            else if (hit.collider != null && hit.collider.CompareTag("CodeArea"))
            {
                // If a collider is hit but it is the CodeArea than move all blocks, the flag is simply not setting block but setting isDragging true
                offset = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                isDragging = true;
            }
        }

        // For dragging and releasing and snapping
        if (isDragging && block != null)
        {
            // Update the object's position based on the mouse position
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            if (dragChildren)
            {
                ((Block)block.GetComponent("Block")).moveChildren(new Vector2(newPosition.x - block.transform.position.x, newPosition.y - block.transform.position.y));
            }
            else
            {
                block.transform.position = new Vector3(newPosition.x, newPosition.y, block.transform.position.z);
            }
            lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Everything beyond has to do with finding snap partners


            // Gets all colliders that overlap with a circle of radius of searchRadius
            List<GameObject> blockList = findCloseBlocks(); ;

            updateCurrentOptions(blockList);

            // Check for mouse button release, handles snapping with located partners
            if (Input.GetMouseButtonUp(0))
            {
                blockScript.setRenderLayersLow();
                misplacedDestroy();
                GameObject scrollArea = GameObject.FindGameObjectWithTag("CodeArea");
                block.transform.SetParent(scrollArea.transform);

                snapToBlocks();

                // Sets the block for next object to be picked up
                block.layer = 0;
                block = null;
                blockScript = null;

                isDragging = false;
            }
        }
        else if (isDragging)
        {
            // Handles the pan functionality

            // Finds all gameobjects that are a block
            GameObject[] blocks = GameObject.FindGameObjectsWithTag("block");
            // Gets the offset of mouse position from the last frame to the current one 
            Vector2 translate = offset - Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // For every gameobject update thier transforms
            foreach (GameObject block in blocks)
            {
                block.transform.position = block.transform.position - new Vector3(translate.x, translate.y, block.transform.position.z);
            }

            // Transform the startBlocks position as well (special case since it doesn't have a tag as a block)
            startBlock.transform.position = startBlock.transform.position - new Vector3(translate.x, translate.y, 0f);

            // Save offset as this frames mouse position
            offset = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Check for mouse button release. Stop panning mouse is released 
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                dragChildren = false;
            }
        }

    }





    private void setRefrences()
    {

        // Sets the refrences of the block and any blocks attached to it to be null.
        Block tempScript = ((Block)block.GetComponent("Block"));
        if (tempScript.prevBlock != null)
        {
            oldPrevBlock = tempScript.prevBlock;
            tempScript.prevBlock.nextBlock = null;
            tempScript.prevBlock = null;

        }
        if (tempScript.nextBlock != null && !dragChildren)
        {
            oldNextBlock = tempScript.nextBlock;
            tempScript.nextBlock.prevBlock = null;
            tempScript.nextBlock = null;
        }
    }


    private List<GameObject> findCloseBlocks()
    {
        // Gets all colliders that overlap with a circle of radius of searchRadius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(block.transform.position, searchRadius);
        List<GameObject> blockList = new List<GameObject>();

        // Iterate through list of found objects to see if any are snappable
        foreach (Collider2D collider in colliders)
        {
            bool isNotConnectedBlock = true;
            Block currentBlock = blockScript;
            while (currentBlock && isNotConnectedBlock)
            {
                isNotConnectedBlock = currentBlock.gameObject != collider.gameObject;
                currentBlock = currentBlock.nextBlock;
            }

            // Identification of blocks is done by tag unless you are the start block. This is because we didn't want the start block to be snappable
            if (collider.CompareTag("block") && isNotConnectedBlock)
            {
                blockList.Add(collider.gameObject);
            }
            // To identify the start block as snappable without tag, checks if it has a script called startBlock
            else if (collider.gameObject.GetComponent("startBlock") != null)
            {
                blockList.Add(collider.gameObject);
            }
        }

        // Sort the list of game objects by distance from 'block'
        blockList = blockList.OrderBy(obj => Vector2.Distance(block.transform.position, obj.transform.position)).ToList();

        return blockList;
    }

    private void misplacedDestroy()
    {
        // Ignore raycast set so that we can check if the block was placed on the main code area
        block.layer = LayerMask.NameToLayer("Ignore Raycast");

        // Raycast from mouse to hit objects behind
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        // If the object that is hit is not the code area or just doesn't exist than it destoys the block
        if (hit.collider == null || !hit.collider.CompareTag("CodeArea"))
        {
            Debug.Log(hit.collider);
            //Destroy(block);
        }
    }

    private void snapToBlocks()
    {
        if (newNextBlock != null)
        {
            Block lastBlock = blockScript.getLastBlock();
            if (insertingBlock)
            {
                Vector2 jump = -newNextBlock.snapPositions[0] + lastBlock.snapPositions[1];
                //new Vector2(blockScript.snapPositions[1].x - newNextBlock.snapPositions[0].x, newNextBlock.snapPositions[0].y - blockScript.snapPositions[1].y)
                newNextBlock.moveChildren(jump);
            }
            else
            {
                Vector2 jump = newNextBlock.snapPositions[0] - lastBlock.snapPositions[1];
                blockScript.moveChildren(jump);
            }

            newNextBlock.prevBlock = lastBlock;
            lastBlock.nextBlock = newNextBlock;
        }
        if (newPrevBlock != null)
        {
            Vector2 jump;
            if (newPrevBlock.blockID == 0)
            {
                jump = newPrevBlock.snapPositions[0] - blockScript.snapPositions[0];
            }
            else
            {
                jump = newPrevBlock.snapPositions[1] - blockScript.snapPositions[0];
            }

            blockScript.moveChildren(jump);

            newPrevBlock.nextBlock = blockScript;
            blockScript.prevBlock = newPrevBlock;
        }

        if (newNextBlock == oldNextBlock || newPrevBlock == oldPrevBlock)
        {
            if (oldNextBlock != null)
            {
                oldNextBlock.prevBlock = blockScript;
                blockScript.nextBlock = oldNextBlock;
                oldNextBlock = null;
            }
            if (oldPrevBlock != null)
            {
                oldPrevBlock.nextBlock = blockScript;
                blockScript.prevBlock = oldPrevBlock;
                oldPrevBlock = null;
            }
        }
        else if (newNextBlock != null || newPrevBlock != null)
        {
            oldPrevBlock = null;
            oldNextBlock = null;
        }
        oldSnapPositions = null;
        insertingBlock = null;
    }


    private void updateCurrentOptions(List<GameObject> blockList)
    {

        if (insertingBlockChanged)
        {
            insertingBlockChanged = false;

            newPrevBlock = insertingBlock;
            newNextBlock = insertingBlock.nextBlock;

            // Standard is that 0 is the old position of the snapPosition with highest y (also one of these positions is actually not old, but it nice to have them in one spot).
            oldSnapPositions = new Vector2[3];

            if (insertingBlock.blockID == 0)
            {
                oldSnapPositions[0] = insertingBlock.snapPositions[0] + new Vector2(0f, 1f);
                oldSnapPositions[1] = insertingBlock.snapPositions[0];
                oldSnapPositions[2] = insertingBlock.nextBlock.snapPositions[1];
            }
            else
            {
                oldSnapPositions[0] = insertingBlock.snapPositions[0];
                oldSnapPositions[1] = insertingBlock.snapPositions[1];
                oldSnapPositions[2] = insertingBlock.nextBlock.snapPositions[1];
            }

            insertingBlock.nextBlock.moveChildren(new Vector2(0, -((Block)block.GetComponent("Block")).getListHeight()));
        }

        if (oldSnapPositions != null)
        {
            float comparison = Vector2.Distance(block.transform.position, oldSnapPositions[1]);
            if (comparison > Vector2.Distance(block.transform.position, oldSnapPositions[0]))
            {
                insertingBlock.nextBlock.moveChildren(new Vector2(0, ((Block)block.GetComponent("Block")).getListHeight()));

                if (insertingBlock.prevBlock != null)
                {
                    insertingBlock = insertingBlock.prevBlock;
                    insertingBlockChanged = true;
                }
                else
                {
                    newPrevBlock = null;
                    newNextBlock = insertingBlock;
                    oldSnapPositions = null;
                    insertingBlock = null;
                }
            }
            else if (comparison > Vector2.Distance(block.transform.position, oldSnapPositions[2]))
            {
                insertingBlock.nextBlock.moveChildren(new Vector2(0, ((Block)block.GetComponent("Block")).getListHeight()));

                if (insertingBlock.nextBlock.nextBlock != null)
                {
                    insertingBlock = insertingBlock.nextBlock;
                    insertingBlockChanged = true;
                }
                else
                {
                    newPrevBlock = insertingBlock.nextBlock;
                    newNextBlock = null;
                    oldSnapPositions = null;
                    insertingBlock = null;
                }
            }
            else if (blockList.Count == 0)
            {
                insertingBlock.nextBlock.moveChildren(new Vector2(0, ((Block)block.GetComponent("Block")).getListHeight()));
                newPrevBlock = null;
                newNextBlock = null;
                oldSnapPositions = null;
                insertingBlock = null;
            }
        }
        else if (blockList.Count > 0)
        {
            Block closestBlock = (Block)blockList[0].GetComponent("Block");

            newPrevBlock = null;
            newNextBlock = null;

            if (closestBlock.blockID == 0 || Vector2.Distance(closestBlock.snapPositions[0], block.transform.position) > Vector2.Distance(closestBlock.snapPositions[1], block.transform.position))
            {
                newPrevBlock = closestBlock;
                if (closestBlock.nextBlock != null)
                {
                    insertingBlock = closestBlock;
                    insertingBlockChanged = true;
                }
            }
            else
            {
                newNextBlock = closestBlock;
                if (closestBlock.prevBlock != null)
                {
                    insertingBlock = closestBlock.prevBlock;
                    insertingBlockChanged = true;
                }
            }
        }
        else
        {
            newPrevBlock = null;
            newNextBlock = null;
            if (insertingBlock)
            {
                oldSnapPositions = null;
                insertingBlock = null;
            }
        }


    }
}