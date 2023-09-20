using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Draggable : Block
{

    private bool isDragging = false; // To track if the object is currently being dragged.
    private Vector3 offset; // Offset between the mouse click position and the object's position.
    private BlockSpawn blockStatic;
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

/*                SnapToNearestBlock();
*/            }
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
}