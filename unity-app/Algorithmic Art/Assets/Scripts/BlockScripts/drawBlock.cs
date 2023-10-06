using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Script for the Draw Block
public class drawBlock : Block
{
    // For future code 
    [SerializeField]
    private TMP_InputField XInput;
    // For future code 
    [SerializeField]
    public TMP_InputField YInput;
    // Offset for snaps above and below
    [SerializeField]
    private float snapOffset = 1f;

    // The x coordinite input 
    public int X;
    // The Y coordinite input 
    public int Y;

    // Sets the starting information for the block, ID, refrences and snap positions
    void Awake()
    {
        this.blockID = Block.nextID;
        this.topSnapped = false;
        this.botSnapped = false;
        this.prevBlock = null;
        this.nextBlock = null;

        Block.nextID++;

        snapPositions = new Vector2[2];
        snapPositions[0] = new Vector2(transform.position.x, transform.position.y - snapOffset);
        snapPositions[1] = new Vector2(transform.position.x, transform.position.y + snapOffset);

        //Debug.Log(snapPositions[0]);
    }

    // Used to update information on the draw block
    void Update()
    {
        // Updates the snap positions with any new position of block
        snapPositions[0] = new Vector2(transform.position.x, transform.position.y - snapOffset);
        snapPositions[1] = new Vector2(transform.position.x, transform.position.y + snapOffset);

        // Checks the x and Y input for valid integers, if non found than sets to default value 1
        // Note: Paramarterize the default value?
        string inputData = XInput.text;
        if (!string.IsNullOrEmpty(inputData))
        {
            if (int.TryParse(inputData, out int parsedX))
            {
                X = parsedX;
            }
            else
            {
                // Handle the case where parsing fails, e.g., set a default value
                X = 1;
            }
        }
        else
        {
            X = 1;
        }

        inputData = YInput.text;
        if (!string.IsNullOrEmpty(inputData))
        {
            if (int.TryParse(inputData, out int parsedY))
            {
                Y = parsedY;
            }
            else
            {
                // Handle the case where parsing fails, e.g., set a default value
                Y = 1;
            }
        }
        else
        {
            Y = 1;
        }
    }

    // Will be used to draw line using a child linerenderer component. Not yet implemented
    public override void execute()
    {
        //TODO: Implement modularized block running
    }
}
