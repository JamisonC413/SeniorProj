using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    [SerializeField]
    private GameObject startBlock;

    private Vector2 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (this.gameObject.activeSelf)
        {
            offset = offset - (Vector2)transform.position;

            // Finds all gameobjects that are a block
            GameObject[] blocks = GameObject.FindGameObjectsWithTag("block");

            // For every gameobject update thier transforms
            foreach (GameObject block in blocks)
            {
                block.transform.position = block.transform.position - new Vector3(offset.x, offset.y, block.transform.position.z);
            }

            // Transform the startBlocks position as well (special case since it doesn't have a tag as a block)
            startBlock.transform.position = startBlock.transform.position - new Vector3(offset.x, offset.y, 0f);

            offset = transform.position;
        }


    }

    public Vector2 getOffset()
    {
        Vector2 currentoffset = offset - (Vector2)transform.position;

        return currentoffset;
    }
}
