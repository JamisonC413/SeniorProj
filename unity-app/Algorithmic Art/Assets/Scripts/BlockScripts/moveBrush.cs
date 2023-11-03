using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Script for the Draw Block
public class moveBrush : Block
{
    // For future code 
    [SerializeField]
    private TMP_InputField XInput;
    // For future code 
    [SerializeField]
    private TMP_InputField YInput;

    [SerializeField]
    private GameObject snap1;

    [SerializeField]
    private GameObject snap2;

    // Default for x and y coordinates
    [SerializeField]
    private int defaultCoords = 0;


    // The x coordinite input 
    public int X;
    // The Y coordinite input 
    public int Y;

    public float scale = 0.1f;

    // The brushes gameobject 
    public Brush brush;


    // Sets the starting information for the block, ID, refrences and snap positions
    void Awake()
    {
        initialize();

        snapPositions = new Vector2[2];
        snapPositions[0] = snap1.transform.position;
        snapPositions[1] = snap2.transform.position;

        //Note: Replaced GameObject.Find with GameObject.FindGameObjectWithTag because Find is very expensive - Tong
        brush = GameObject.FindGameObjectWithTag("brush").GetComponent<Brush>();
    }

    // Used to update information on the draw block
    void Update()
    {
        // Updates the snap positions with any new position of block
        snapPositions[0] = snap1.transform.position;
        snapPositions[1] = snap2.transform.position;

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
                X = defaultCoords;
            }
        }
        else
        {
            X = defaultCoords;
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
                Y = defaultCoords;
            }
        }
        else
        {
            Y = defaultCoords;
        }

    }

    public void initialize()
    {
        this.blockID = Block.nextID;
        this.prevBlock = null;
        this.nextBlock = null;

        Block.nextID++;
    }

    // Will be used to draw line using a child linerenderer component. Not yet implemented
    public override void execute()
    {

        float xTransform = X * scale + brush.transform.position.x;
        float yTransform = Y * scale + brush.transform.position.y;

        // Create bounds for the lines
        if (xTransform < brush.startPosition.x)
        {
            xTransform = brush.startPosition.x;
        }
        if (yTransform < brush.startPosition.y)
        {
            yTransform = brush.startPosition.y;
        }
        if (xTransform > brush.startPosition.x + brush.drawArea.x)
        {
            xTransform = brush.startPosition.x + brush.drawArea.x;
        }
        if (yTransform > brush.startPosition.y + brush.drawArea.y)
        {
            yTransform = brush.startPosition.y + brush.drawArea.y;
        }

        // Transform the brush
        brush.transform.position = new Vector3(xTransform, yTransform, 0f);
    }
}