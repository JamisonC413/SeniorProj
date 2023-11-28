using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// Handles moving and snapping blocks.
public class blockMover : MonoBehaviour
{
    // Offset tracks the difference between the mouse position and the transform of the block
    private Vector3 offset;

    // Tracks if a block is getting dragged
    private bool isDragging = false;

    // The search radius for finding blocks to snap to
    public float searchRadius = .1f;

    // The block currently getting dragged
    // NOTE: expirimented with removing isDragging and replacing it with a null check on this was done... but it didn't work
    //       maybe someone else will have a better idea?
    public GameObject block;

    // The script for the block currently being dragged
    private Block blockScript = null;

    // Tracks if a block is getting inserted into the middle of a list
    public Block insertingBlock = null;

    // Tracks the frame that insertingBlock gets changed
    private bool insertingBlockChanged = false;

    // Old positions of key block snaps for inserting a block
    private Vector2[] oldSnapPositions = null;

    // The new previous block in the list, will be attached to the prev of block
    public Block newPrevBlock = null;

    // The new next block in the list, will be attached to the prev of block
    public Block newNextBlock = null;

    // Camera for maximized canvas view
    public Camera cameraMax;

    // Current Camera that is active
    public Camera currentCamera = null;

    // Flag for dragging the block individually or all children as well
    private bool dragChildren = false;

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
                currentCamera = Camera.main;
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                
            }
            else if (cameraMax != null && cameraMax.isActiveAndEnabled)
            {
                currentCamera = cameraMax;
                ray = cameraMax.ScreenPointToRay(Input.mousePosition);
            }

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            // Helpful debug code, tells you what object gets hit by raycast
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.gameObject);
            }

            // If the raycast collides with object than check the tag for block
            if (hit.collider != null && hit.collider.CompareTag("text"))
            {

            }
            else if (hit.collider != null && hit.collider.CompareTag("block"))
            {
                // Store the block reference as the block we are dragging and calculate the offset
                block = hit.collider.gameObject;

                blockScript = block.GetComponent<Block>();

                if (blockScript is not nestedBottom)
                {

                    offset = block.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);


                    // Set isDragging to true
                    isDragging = true;

                    // If a block is nested - drag Children
                    // If a block has
                    if (((Block)block.GetComponent("Block")).prevBlock && (NestedBlock)block.GetComponent("NestedBlock") == null)
                    {
                        dragChildren = false;
                    }
                    else
                    {
                        dragChildren = true;
                    }
                    // Sets refrences of the block to null, also sets the refrences of blocks being broken away from
                    setRefrences();
                    //blockScript.setRenderLayersHigh();
                }
                else
                {
                    block = null;
                    blockScript = null;
                }
            }
            else if (hit.collider != null )
            {
                // If a collider is hit but it is the CodeArea than move all blocks, the flag is simply not setting block but setting isDragging true
                offset = currentCamera.ScreenToWorldPoint(Input.mousePosition);
                isDragging = true;
            }
        }

        // For dragging and releasing and snapping
        if (isDragging && block != null)
        {
            // Update the object's position based on the mouse position
            Vector3 newPosition = currentCamera.ScreenToWorldPoint(Input.mousePosition) + offset;
            if (dragChildren)
            {
                ((Block)block.GetComponent("Block")).moveChildren(new Vector2(newPosition.x - block.transform.position.x, newPosition.y - block.transform.position.y));
            }
            else
            {
                block.transform.position = new Vector3(newPosition.x, newPosition.y, block.transform.position.z);
            }

            // Everything beyond has to do with finding snap partners


            // Gets all colliders that overlap with a circle of radius of searchRadius
            List<GameObject> blockList = findCloseBlocks(); ;

            // Updates the options for nearby blocks to snap to
            updateCurrentOptions(blockList);

            // Check for mouse button release, handles snapping with located partners
            if (Input.GetMouseButtonUp(0))
            {
                //blockScript.setRenderLayersLow();
                misplacedDestroy();
                GameObject scrollArea = GameObject.FindGameObjectWithTag("CodeArea");
                // Snaps to available blocks
                snapToBlocks();

                // Sets the block for next object to be picked up
                block.layer = 0;
                block = null;
                blockScript = null;

                isDragging = false;
            }
        }

    }




    // Sets refrences to blocks that were attached, used when picking up a new block
    private void setRefrences()
    {
        if ((NestedBlock)block.GetComponent("NestedBlock") != null && ((NestedBlock)blockScript).bottomBlock.nextBlock != null)
        {
            // If the block is in the middle of a list and there is not a flag for dragging children than completely disconnect 
            // from the middle of the list
            Block bottomBlock = ((NestedBlock)blockScript).bottomBlock;
            Block nextBlock = bottomBlock.nextBlock;
            Block prevBlock = blockScript.prevBlock;

            if (nextBlock)
            {
                bottomBlock.nextBlock = null;
                nextBlock.prevBlock = null;
            }
            if (prevBlock)
            {
                blockScript.prevBlock = null;
                prevBlock.nextBlock = null;
            }


            if (prevBlock && nextBlock)
            {
                prevBlock.nextBlock = nextBlock;
                nextBlock.prevBlock = prevBlock;

                // The block beneath the block being removed gets bumped up in the list, filling the vacancy of the removed block
                Vector2 jump;
                if (prevBlock.snapPositions.Length == 1)
                {
                    jump = -nextBlock.snapPositions[0] + prevBlock.snapPositions[0];
                }
                else
                {
                    jump = -nextBlock.snapPositions[0] + prevBlock.snapPositions[1];
                }
                nextBlock.moveChildren(jump);
            }

        }
        // Sets the refrences of the block and any blocks attached to it to be null. Depends on a few factors
        else if (blockScript.prevBlock && blockScript.nextBlock && !dragChildren)
        {
            // If the block is in the middle of a list and there is not a flag for dragging children than completely disconnect 
            // from the middle of the list
            Block nextBlock = blockScript.nextBlock;
            Block prevBlock = blockScript.prevBlock;
            prevBlock.nextBlock = nextBlock;
            nextBlock.prevBlock = prevBlock;

            // The block beneath the block being removed gets bumped up in the list, filling the vacancy of the removed block
            Vector2 jump;
            if (prevBlock.snapPositions.Length == 1)
            {
                jump = -nextBlock.snapPositions[0] + prevBlock.snapPositions[0];
            }
            else
            {
                jump = -nextBlock.snapPositions[0] + prevBlock.snapPositions[1];
            }
            nextBlock.moveChildren(jump);

            // The block getting removed has all refrences reset, ready to be snapped somewhere else
            blockScript.prevBlock = null;
            blockScript.nextBlock = null;
        }
        // These cases don't have to deal with removing from the middle of a list, only one of them should be executed if the case above was not
        else if(blockScript.prevBlock != null)
        {
            // If the block has a previous block than reset it
            blockScript.prevBlock.nextBlock = null;
            blockScript.prevBlock = null;
        }
        else if (blockScript.nextBlock != null && !dragChildren)
        {
            // If the block has a next block than reset it 
            blockScript.nextBlock.prevBlock = null;
            blockScript.nextBlock = null;
        }
    }

    // Finds nearby blocks and determines if they are valid to snap to
    private List<GameObject> findCloseBlocks()
    {
        // Gets all colliders that overlap with a circle of radius of searchRadius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(block.transform.position, searchRadius);
        List<GameObject> blockList = new List<GameObject>();

        // Iterate through list of found objects to see if any are snappable
        foreach (Collider2D collider in colliders)
        {
            // Bool for checking that the block is not one of the children blocks to the block that is being moved (only applies when picking up a list of blocks)
            bool isNotConnectedBlock = true;
            Block currentBlock = blockScript;
            // Checks that the current block is not connected to the block being dragged
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
            // To identify the start block as snappable without tag, compares to the existing refrence
            else if (collider.gameObject.GetComponent<startBlock>() != null)
            {
                blockList.Add(collider.gameObject);
            }
        }

        // Sort the list of game objects by distance from 'block'
        blockList = blockList.OrderBy(obj => Vector2.Distance(block.transform.position, obj.transform.position)).ToList();

        return blockList;
    }

    // Destroys the block when it is dragged over the wrong area.
    private void misplacedDestroy()
    {
        // Ignore raycast set so that we can check if the block was placed on the main code area
        block.layer = LayerMask.NameToLayer("Ignore Raycast");

        // Raycast from mouse to hit objects behind
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        // If the object that is hit is not the code area or just doesn't exist than it destoys the block

        //if (hit.collider == null && (!hit.collider.CompareTag("CodeArea") || !hit.collider.CompareTag("Block")))
        //{
            //Debug.Log(hit.collider);
            //Block current = blockScript.getLastBlock();
            //while (current != null)
            //{
            //    current = current.prevBlock;
            //    Destroy(current.gameObject);
            //}
        //}
    }

    // Snaps to the blocks selected to be the newPrevious and newNext
    private void snapToBlocks()
    {
        // If newNextBlock exists than move the block to the top of the next block, if it is being inserted than move the nextBlock to the bottom of this block
        if (newNextBlock != null)
        {
            Block lastBlock = blockScript.getLastBlock();
            if (insertingBlock)
            {
                // Handles calculating offset to move the block
                Vector2 jump = -newNextBlock.snapPositions[0] + lastBlock.snapPositions[1];
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
        // If there is a newPreviousBlock than jump this block to the bottom snap position of it
        if (newPrevBlock != null)
        {
            Vector2 jump;
            // Calculate jump (special case for the startBlock) StartBlock always has the id == 0
            if (newPrevBlock.snapPositions.Length == 1)
            {
                jump = newPrevBlock.snapPositions[0] - blockScript.snapPositions[0];
            }
            else
            {
                jump = newPrevBlock.snapPositions[1] - blockScript.snapPositions[0];
            }

            // Move this block and all children to the bottom of the newPreviousBlock
            blockScript.moveChildren(jump);

            // Sets refrences, creating list
            newPrevBlock.nextBlock = blockScript;
            blockScript.prevBlock = newPrevBlock;
        }


        oldSnapPositions = null;
        insertingBlock = null;
    }

    // Updates the options for blocks based off what is connected to nearby blocks 
    private void updateCurrentOptions(List<GameObject> blockList)
    {
        // If the inserting block got changed than updates options pertaining to a insert case
        if (insertingBlockChanged)
        {
            // Reset the flag
            insertingBlockChanged = false;

            // By default the inserting block is the block that will be above the dragged block i.e. newPrevBlock
            newPrevBlock = insertingBlock;

            // By aboves default, the nextBlock of the insertBlock will be the newNext.
            
            // NOTE: It should be impossible for insertingBlock to get changed
            // and for there to not be a nextBlock
            // (insertingBlockChanged flag should only be true when user tries inserting into the middle of a list),
            // if this line gives a null than logic elsewhere got broken.
            newNextBlock = insertingBlock.nextBlock;

            // Standard is that 0 is the old position of the snapPosition with highest y (also one of these positions is actually not old, but it nice to have them in one spot).
            oldSnapPositions = new Vector2[3];

            // If the block is the startBlock there is a special case for the triggering snap positions. This is because start doesn't have two snapPostions
            if (insertingBlock.snapPositions.Length == 1)
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

            // Moves the blocks beneath the insertingBlock down, making room for new block 
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
        // If there are nearby blocks
        else if (blockList.Count > 0 && blockScript.snapPositions.Length != 1)
        {
            // Get the closest block, determines case for connecting based on this block
            Block closestBlock = (Block)blockList[0].GetComponent("Block");

            newPrevBlock = null;
            newNextBlock = null;

            // If the closest block is the start block or the distance to the bottom snapPoint is in snapPositions[0] than execute this case
            if (closestBlock.snapPositions.Length == 1 || Vector2.Distance(closestBlock.snapPositions[0], block.transform.position) > Vector2.Distance(closestBlock.snapPositions[1], block.transform.position))
            {
                // Sets the newPrevBlock
                newPrevBlock = closestBlock;
                // Raise flag for insertion if the nextBlock slot is already taken
                if (closestBlock.nextBlock != null)
                {
                    insertingBlock = closestBlock;
                    insertingBlockChanged = true;
                }
            }
            else
            {
                // Must be closer to top of block, so sets newNextBlock
                newNextBlock = closestBlock;
                // If the prevBlock refrence is taken than set the inserting block to the prevBlock refrence
                // NOTE: The decision to make insertingBlock always the newPrevBlock is important to keeping the 
                // inserting block code simpler. That is why here we set the insertingBlock equal to closestBlock's previous
                // while above we simply make it the closest block.
                if (closestBlock.prevBlock != null)
                {
                    insertingBlock = closestBlock.prevBlock;
                    insertingBlockChanged = true;
                }
            }
        }
        else
        {
            // No blocks are nearby, reset all prospective options
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