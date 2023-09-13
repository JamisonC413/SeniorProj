using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : Block
{

    private bool isDragging = false; // To track if the object is currently being dragged.
    private Vector3 offset; // Offset between the mouse click position and the object's position.
    private BlockStaticScript blockStatic;
    private bool isSnapped = false;
    private Vector3[] snapPoints; // Define snap points here.

    // Start is called before the first frame update

    void Start()
    {
        snapPoints = new Vector3[]
        {
            new Vector3(0.0f, -GetComponent<Collider2D>().bounds.extents.y, 0.0f)
        };
    }

    void Awake()
    {
        this.blockID = Block.nextID;
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
                Debug.Log("4 end drag");

                // Stop dragging when the mouse button is released.
                isDragging = false;

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
                    Debug.Log("4 start drag");
                    // Calculate the offset between the click position and the object's position.
                    offset = transform.position - MouseWorldPos();
                    isDragging = true;
                    isSnapped = false;
                }

            }

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
        float xSnapDistance = 0.8f;
        float ySnapDistance = 1.3f;

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

                    if (yDistance <= 0)
                    {
                        newYPosition = block.transform.position.y - (GetComponent<BoxCollider2D>().size.y / 4);
                        Debug.Log("snapped below");

                    }
                    else
                    {
                        newYPosition = block.transform.position.y + (GetComponent<BoxCollider2D>().size.y / 4);
                        Debug.Log("snapped on top");
                    }

                    transform.position = new Vector3(block.transform.position.x, newYPosition, block.transform.position.z);


                }
            }
        }

        // If the object didn't snap to any snap point, you can handle it differently (e.g., return it to its original position).
        if (!isSnapped)
        {
            // Handle the case when the object is not snapped.
        }
    }
}
