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
    private TMP_InputField YInput;
    // Offset for snaps above and below
    [SerializeField]
    private float snapOffset = 1f;

    // Default for x and y coordinates
    [SerializeField]
    private int defaultCoords = 0;


    // The x coordinite input 
    public int X;
    // The Y coordinite input 
    public int Y;

    // The brushes gameobject 
    public Brush brush;

    //The LineRenderer
    public LineRenderer lineRenderer;

    // Positions of points to draw in lineRenderer
    private List<Vector3> positions = new List<Vector3>();


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

        brush = GameObject.Find("Brush").GetComponent<Brush>();
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

    // Will be used to draw line using a child linerenderer component. Not yet implemented
    public override void execute()
    {
        // Clear the list of positions
        positions.Clear();

        lineRenderer = brush.createLineRenderer();

        // Add a origin point
        positions.Add(brush.transform.position);

        float xTransform = X + brush.transform.position.x;
        float yTransform = Y + brush.transform.position.y;

        // Create bounds for the lines
        if (xTransform < brush.startPosition.x)
        {
            xTransform = brush.startPosition.x;
        }
        if (yTransform < brush.startPosition.y)
        {
            yTransform = brush.startPosition.y;
        }
        if(xTransform > brush.startPosition.x + brush.drawArea.x)
        {
            xTransform = brush.startPosition.x + brush.drawArea.x;
        }
        if (yTransform > brush.startPosition.y + brush.drawArea.y)
        {
            yTransform = brush.startPosition.y + brush.drawArea.y;
        }

        // Misc Debug
        Debug.Log(new Vector3(xTransform, yTransform, 0f));

        // Add the point from the block to the line renderer
        positions.Add(new Vector3(xTransform, yTransform, 0f));

        brush.transform.position = new Vector3(xTransform, yTransform, 0f);

        // Render lines
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());

    }
}
