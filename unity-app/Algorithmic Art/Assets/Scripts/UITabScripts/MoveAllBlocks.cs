using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveAllBlocks : MonoBehaviour
{

    // Moves start block and all blocks between minimized and maximized views
    public static void moveAllBlocks()
    {
        // Finds all the relevant block objects
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("block");
        GameObject start = GameObject.FindGameObjectWithTag("start");

        // Check if minimized or maximized view
        if (ChangeCamera.minimized)
        {

            // Moves all blocks to the maximized view big canvas
            start.transform.position = start.transform.position + new Vector3(27, 0, 0);

            foreach (GameObject block in blocks)
            {
                block.transform.position = block.transform.position + new Vector3(27, 0, 0);
            }

            // Useful for debugging
            //Debug.Log("min to max view");

        }
        else if (ChangeCamera.maximized)
        {
            start.transform.position = start.transform.position + new Vector3(-27, 0, 0);

            // Moves all blocks to the minimized view small canvas
            foreach (GameObject block in blocks)
            {
                block.transform.position = block.transform.position + new Vector3(-27, 0, 0);
            }

            // Useful for debugging
            //Debug.Log("max to min view");
        }
    }
}
