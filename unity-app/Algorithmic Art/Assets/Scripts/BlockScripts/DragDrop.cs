using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DragDrop : Block
{

    private bool isDragging = false; // To track if the object is currently being dragged.
    private Vector3 offset; // Offset between the mouse click position and the object's position.
    private BlockStaticScript blockStatic;
    private bool isSnapped = false;
    private Vector3[] snapPoints; // Define snap points here.
    private bool isInsideDropZone = false;

    // Start is called before the first frame update

    void Start()
    {
        //snapPoints = new Vector3[]
        //{
        //    new Vector3(0.0f, -GetComponent<Collider2D>().bounds.extents.y, 0.0f)
        //};
    }

    void Awake()
    {
        this.blockID = Block.nextID;
        this.topSnapped = false;
        this.botSnapped = false;
        this.prevBlock = null;
        this.nextBlock = null;
        Debug.Log("Block ID: " + this.blockID);

        Block.nextID++;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            // Move the object to the calculated position with the offset.
            transform.position = MouseWorldPos() + offset;

            if (Input.GetMouseButtonUp(0))
            {
                //gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

                //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                //if (hit.collider == null)
                //{
                //    Debug.Log("hit : null");
                //    Destroy(gameObject);
                //}


                //if (hit.collider != null && !hit.collider.CompareTag("CodeArea"))
                //{
                //    Debug.Log("hit : " + hit.collider.tag);
                //    //Destroy(gameObject);
                //}

                //gameObject.layer = LayerMask.NameToLayer("BlockLayer");


                isDragging = false;

                if (!isInsideDropZone)
                {
                    Destroy(gameObject);
                }

                Debug.Log("end drag");

                // Stop dragging when the mouse button is released.
                GetComponent<SpriteRenderer>().sortingOrder = 1;

                SnapToNearestBlock();
            }
        }
        else
        {
            // If the mouse is over the object and the left mouse button is pressed, start dragging.
            if (Input.GetMouseButtonDown(0))
            {
                // Cast a ray from the camera to the mouse position.
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    //Debug.Log("start drag");
                    // Calculate the offset between the click position and the object's position.
                    offset = transform.position - MouseWorldPos();

                    GetComponent<SpriteRenderer>().sortingOrder = 2;

                    isDragging = true;
                    isSnapped = false;
                    this.topSnapped = false;
                    this.botSnapped = false;

                    if (prevBlock != null)
                    {
                        prevBlock.botSnapped = false;
                        prevBlock.nextBlock = null;
                        Debug.Log(prevBlock.blockID + " bot snap cleared");
                    }
                    if (nextBlock != null)
                    {
                        nextBlock.topSnapped = false;
                        nextBlock.prevBlock = null;
                        Debug.Log(nextBlock.blockID + " top snap cleared");
                    }

                }

            }

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("enter trigger");

        if (other.CompareTag("CodeArea"))
        {
            isInsideDropZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("exit trigger");

        if (other.CompareTag("CodeArea"))
        {
            isInsideDropZone = false;
        }
    }

    Vector3 MouseWorldPos()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }

    void SnapToNearestBlock()
    {
        float xSnapDistance = 1f;
        float ySnapDistance = 1.5f;

        Block[] blocks = FindObjectsOfType<Block>();

        foreach (Block block in blocks)
        {
            // Ensure we don't snap to ourselves
            if (block.blockID != this.blockID)
            {
                float xDistance = Mathf.Abs(transform.position.x - block.transform.position.x);
                float yDistance = transform.position.y - block.transform.position.y;

                if (xDistance < xSnapDistance && Mathf.Abs(yDistance) < ySnapDistance)
                {


                    // Snap this block to the other block
                    float newYPosition = 0;

                    if (yDistance <= 0 && block.botSnapped == false)
                    {
                        newYPosition = block.transform.position.y - (GetComponent<BoxCollider2D>().size.y / 1.75f);
                        Debug.Log("snapped below");
                        //makes sure blocks dont snap on already snapped blocks
                        block.botSnapped = true;
                        this.topSnapped = true;
                        prevBlock = block;
                        block.nextBlock = this;
                        //snaps block
                        transform.position = new Vector3(block.transform.position.x, newYPosition, block.transform.position.z);

                    }
                    if (yDistance > 0 && block.topSnapped == false)
                    {
                        newYPosition = block.transform.position.y + (GetComponent<BoxCollider2D>().size.y / 1.75f);
                        Debug.Log("snapped on top");
                        //makes sure blocks dont snap on already snapped blocks
                        block.topSnapped = true;
                        this.botSnapped = true;
                        nextBlock = block;
                        block.prevBlock = this;
                        //snaps block
                        transform.position = new Vector3(block.transform.position.x, newYPosition, block.transform.position.z);
                    }


                }
            }
        }
    }
}
