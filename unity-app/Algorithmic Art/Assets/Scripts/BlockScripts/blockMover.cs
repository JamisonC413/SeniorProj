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

    // Update is called every frame and handles dragging and snapping
    void Update()
    {
        // Split into two main sections, button down to initiate dragging and a isDragging section

        // When button is pressed checks if there is a block underneath and then grabs it
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the mouse position and grab the first object hit
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
                offset = block.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);

                // Set isDragging to true
                isDragging = true;

                // Gets the rendering component of the block and sets its layer to be above all UI 
                // NOTE: MAKE INTO FUNCTION!?
                Renderer rendererComponent = block.GetComponent<Renderer>();
                if (rendererComponent != null)
                {
                    rendererComponent.sortingLayerName = "Block";
                }

                // Sets refrences of the block to null, also sets the refrences of blocks being broken away from
                Block tempScript = ((Block)block.GetComponent("drawBlock"));
                if (tempScript.prevBlock != null)
                {
                    tempScript.prevBlock.nextBlock = null;
                    tempScript.prevBlock = null;
                }
                if(tempScript.nextBlock != null)
                {
                    tempScript.nextBlock.prevBlock = null;
                    tempScript.nextBlock = null;
                }
            }
        }


        // For dragging and releasing and snapping
        if (isDragging && block != null)
        {
            // Update the object's position based on the mouse position
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            block.transform.position = new Vector3(newPosition.x, newPosition.y, block.transform.position.z);


            // Everything beyond has to do with finding snap partners
            // NOTE: Need to split stuff into functions after issue is resolved

            // Get render component for changing render layer
            // NOTE: merge functionality with L52?
            Renderer rendererComponent = block.GetComponent<Renderer>();


            // Gets all colliders that overlap with a circle of radius of searchRadius
            // NOTE: MAKE INTO FUNCTION!?
            Collider2D[] colliders = Physics2D.OverlapCircleAll(block.transform.position, searchRadius);
            List<GameObject> blockList = new List<GameObject>();

            // Iterate through list of found objects to see if any are snappable
            foreach (Collider2D collider in colliders)
            {
                // Identification of blocks is done by tag unless you are the start block. This is because we didn't want the start block to be snappable
                if (collider.CompareTag("block") && collider.gameObject != block)
                {
                    blockList.Add(collider.gameObject);
                }
                // To identify the start block as snappable without tag, checks if it has a script called startBlock
                else if(collider.gameObject.GetComponent("startBlock") != null)
                {
                    blockList.Add(collider.gameObject);
                }
            }

            // Sort the list of game objects by distance from 'block'
            blockList = blockList.OrderBy(obj => Vector2.Distance(block.transform.position, obj.transform.position)).ToList();

            // Check for mouse button release, handles snapping with located partners
            if (Input.GetMouseButtonUp(0))
            {
                // Sets the blocks' rendering layer back to a normal blocks so that it appears under UI, palette and canvas
                rendererComponent.sortingLayerName = "Block Background";
                rendererComponent.sortingOrder = 1;

                // Ignore raycast set so that we can check if the block was placed on the main code area
                block.layer = LayerMask.NameToLayer("Ignore Raycast");

                // Raycast from mouse to hit objects behind
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                // If the object that is hit is not the code area or just doesn't exist than it destoys the block
                if (hit.collider == null || !hit.collider.CompareTag("CodeArea"))
                {
                    Destroy(block);
                }

                // Checks if the block had to jump to a snap point
                bool jumped = false;

                // Deals with snapping
                foreach (GameObject obj in blockList)
                {

                    //Debug.Log(obj.GetComponent("drawBlock"));
                    Vector2 move = Vector2.zero;

                    //Hopefully there is a way to remove this checking for types of block
                    // Get the nearest block and snap to its closest position
                    if (obj.GetComponent("drawBlock") != null)
                    {
                        // If the object is a draw block than get the draw block script
                        drawBlock script = (drawBlock) obj.GetComponent("drawBlock");

                        // Jump to nearest snap position
                        if (Vector2.Distance(script.snapPositions[0], block.transform.position) > Vector2.Distance(script.snapPositions[1], block.transform.position))
                        {
                            if(script.prevBlock == null)
                            {
                                Vector2 jump = script.snapPositions[1] - ((drawBlock)block.GetComponent("drawBlock")).snapPositions[0];
                                block.transform.position = new Vector3(block.transform.position.x + jump.x, block.transform.position.y + jump.y, block.transform.position.z);
                                jumped = true;
                            }
                        }
                        else
                        {
                            if (script.nextBlock == null)
                            {
                                Vector2 jump = script.snapPositions[0] - ((drawBlock)block.GetComponent("drawBlock")).snapPositions[1];
                                block.transform.position = new Vector3(block.transform.position.x + jump.x, block.transform.position.y + jump.y, block.transform.position.z);
                                jumped = true;
                            }
                        }

                        script.nextBlock = (Block)block.GetComponent("drawBlock");
                        ((Block)block.GetComponent("drawBlock")).prevBlock = script;

                    }
                    else if(obj.GetComponent("startBlock") != null)
                    {
                        // If the object is a start block than get the start block script
                        startBlock script = (startBlock)obj.GetComponent("startBlock");

                        // Jump to nearest snap position
                        if (script.nextBlock == null)
                        {
                            Vector2 jump = script.snapPositions[0] + ((drawBlock)block.GetComponent("drawBlock")).snapPositions[0];
                            block.transform.position = obj.transform.position - new Vector3(0f, 2f, 0f);   


                            script.nextBlock = (Block)block.GetComponent("drawBlock");
                            ((Block)block.GetComponent("drawBlock")).prevBlock = script;
                            jumped = true;
                        }
                    }
                }
                
                // Deals with blocks that were placed near blocks and didn't jump. THIS IS A TEMP FEATURE
                if (!jumped && blockList.Count > 0)
                {
                    Destroy(block);
                }

                // Sets the block for next object to be picked up
                block.layer = 0;
                block = null;

                isDragging = false;
            }
        }

    }


}
