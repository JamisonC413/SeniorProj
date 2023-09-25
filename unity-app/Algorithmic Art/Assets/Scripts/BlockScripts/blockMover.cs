using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;
using static UnityEditor.Experimental.GraphView.GraphView;

public class blockMover : MonoBehaviour
{
    private Vector3 offset; 
    private bool isDragging = false;
    public float searchRadius = .1f;
    public GameObject block;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                Debug.Log(hit.collider.gameObject);
            }

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
            }
        }


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
            }

            // Sort the list of game objects by distance from 'block'
            blockList = blockList.OrderBy(obj => Vector2.Distance(block.transform.position, obj.transform.position)).ToList();


            //Debug.Log(blockList.ToArray().Length);




            // Update the object's position based on the mouse position
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            block.transform.position = new Vector3(newPosition.x, newPosition.y, block.transform.position.z);

            // Check for mouse button release
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


                // Deals with snapping
                foreach (GameObject obj in blockList)
                {
                    Debug.Log(obj.GetComponent("drawBlock"));
                    Vector2 move = Vector2.zero;

                    if (obj.GetComponent("drawBlock") != null)
                    {
                        drawBlock script = (drawBlock) obj.GetComponent("drawBlock");
                        if (Vector2.Distance(script.snapPositions[0], block.transform.position) > Vector2.Distance(script.snapPositions[1], block.transform.position))
                        {
                            Vector2 jump = script.snapPositions[1] - ((drawBlock)block.GetComponent("drawBlock")).snapPositions[0];
                            block.transform.position = new Vector3(block.transform.position.x + jump.x, block.transform.position.y + jump.y, block.transform.position.z);
                        }
                        else
                        {
                            Vector2 jump = script.snapPositions[0] - ((drawBlock)block.GetComponent("drawBlock")).snapPositions[1];
                            block.transform.position = new Vector3(block.transform.position.x + jump.x, block.transform.position.y + jump.y, block.transform.position.z);
                        }
                    }
                    else if(obj.GetComponent("startBlock") != null)
                    {

                    }
                    //Vector2.Distance();
                }







                block.layer = 0;
                block = null;

                // Reset the flag and perform any cleanup
                isDragging = false;
            }
        }

    }


}
