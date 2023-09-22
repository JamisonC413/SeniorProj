using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;

public class DraggableObject : MonoBehaviour
{
    private Vector3 offset; // Offset between mouse click and object center
    private bool isDragging = false;
    public float searchRadius = 5f; // Adjust the search radius as needed.
    
    private void OnMouseDown()
    {
        // Calculate the offset between the mouse click point and the object's position
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isDragging = true;
    }

    void Awake( ) 
    { 
        isDragging = true;
    }

    void Update()
    {
        if (isDragging)
        {
            // Update the object's position based on the mouse position
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, searchRadius);
            List<GameObject> blockList = new List<GameObject>();

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("block"))
                {
                    blockList.Add(collider.gameObject);
                }
            }

            // To remove a specific GameObject from the blockList
            GameObject gameObjectToRemove = this.gameObject; // Replace 'this.gameObject' with the specific GameObject you want to remove

            if (blockList.Contains(gameObjectToRemove))
            {
                blockList.Remove(gameObjectToRemove);
            }

            Debug.Log(blockList.ToArray().Length); 

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
        }
    }

}
