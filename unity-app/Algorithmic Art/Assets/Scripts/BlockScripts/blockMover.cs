using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class blockMover : MonoBehaviour
{
    private Vector3 offset; 
    private bool isDragging = false;
    public float searchRadius = .1f;
    public GameObject block;

    void Update()
    {
        // When button is pressed checks if there is a block underneath and then grabs it
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            //if (hit.collider != null)
            //{
            //    Debug.Log(hit.collider.gameObject);
            //}

            if (hit.collider != null && hit.collider.CompareTag("block"))
            {

                // Store the block reference and calculate the offset
                block = hit.collider.gameObject;
                offset = block.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                isDragging = true;

                Renderer rendererComponent = block.GetComponent<Renderer>();
                if (rendererComponent != null)
                {
                    rendererComponent.sortingLayerName = "Block";
                }

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
            Renderer rendererComponent = block.GetComponent<Renderer>();


            // This section deals with getting any nearby blocks and snapping to them, should be moved to a function
            Collider2D[] colliders = Physics2D.OverlapCircleAll(block.transform.position, searchRadius);
            List<GameObject> blockList = new List<GameObject>();

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("block") && collider.gameObject != block)
                {
                    blockList.Add(collider.gameObject);
                }
                else if(collider.gameObject.GetComponent("startBlock") != null)
                {
                    blockList.Add(collider.gameObject);
                }
            }

            // Sort the list of game objects by distance from 'block'
            blockList = blockList.OrderBy(obj => Vector2.Distance(block.transform.position, obj.transform.position)).ToList();


            // Update the object's position based on the mouse position
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            block.transform.position = new Vector3(newPosition.x, newPosition.y, block.transform.position.z);

            // Check for mouse button release, handles snapping
            if (Input.GetMouseButtonUp(0))
            {
                rendererComponent.sortingLayerName = "Block Background";
                rendererComponent.sortingOrder = 1;

                block.layer = LayerMask.NameToLayer("Ignore Raycast");

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit.collider == null)
                {
                    Destroy(block);
                }
                else if (!hit.collider.CompareTag("CodeArea"))
                {
                    Destroy(block);
                }

                bool jumped = false;

                // Deals with snapping
                foreach (GameObject obj in blockList)
                {

                    //Debug.Log(obj.GetComponent("drawBlock"));
                    Vector2 move = Vector2.zero;

                    //Hopefully there is a way to remove this checking for types of block
                    if (obj.GetComponent("drawBlock") != null)
                    {
                        drawBlock script = (drawBlock) obj.GetComponent("drawBlock");
                        //Debug.Log(script.blockID);

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
                        startBlock script = (startBlock)obj.GetComponent("startBlock");

                        if (script.nextBlock == null)
                        {
                            //Debug.Log(script.snapPositions[0]);
                            Vector2 jump = script.snapPositions[0] + ((drawBlock)block.GetComponent("drawBlock")).snapPositions[0];
                            block.transform.position = obj.transform.position - new Vector3(0f, 2f, 0f);   

                            //new Vector3(script.snapPositions[0].x + jump.x, script.snapPositions[0].y + jump.y, block.transform.position.z);
                            //Debug.Log(block.transform.position);
                            //Debug.Log(((drawBlock)block.GetComponent("drawBlock")).snapPositions[0]);

                            script.nextBlock = (Block)block.GetComponent("drawBlock");
                            ((Block)block.GetComponent("drawBlock")).prevBlock = script;
                            jumped = true;
                        }
                    }
                }
                if (!jumped && blockList.Count > 0)
                {
                    Destroy(block);
                }


                block.layer = 0;
                block = null;

                isDragging = false;
            }
        }

    }


}
