using System;
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
    private Vector2 snapOffset = new Vector2(0f, 1f);

    // The brushes gameobject 
    public Brush brush;

    // Contains the data that drawBlock needs, uses this and mode to determine the shape and the details of the shape
    public int[] data;

    // Determines the shape that will get drawn
    // 0 = line
    // 1 = rectangle
    public int mode = 0;

    public Play play;

    // Sets the starting information for the block, ID, refrences and snap positions
    void Awake()
    {
        initialize();

        snapPositions = new Vector2[2];
        snapPositions[0] = new Vector2(transform.position.x, transform.position.y) + snapOffset;
        snapPositions[1] = new Vector2(transform.position.x + snapOffset.x, transform.position.y - snapOffset.y);

        brush = GameObject.Find("Brush").GetComponent<Brush>();
    }

    // Used to update information on the draw block
    void Update()
    {
        // Updates the snap positions with any new position of block
        snapPositions[0] = new Vector2(transform.position.x, transform.position.y) + snapOffset;
        snapPositions[1] = new Vector2(transform.position.x + snapOffset.x, transform.position.y - snapOffset.y);

    }

    public void initialize()
    {
        this.blockID = Block.nextID;
        this.topSnapped = false;
        this.botSnapped = false;
        this.prevBlock = null;
        this.nextBlock = null;

        Block.nextID++;

        play = GameObject.Find("Play").GetComponent<Play>();
    }

    // Will be used to draw line using a child linerenderer component. Not yet implemented
    public override void execute()
    {


        switch (mode)
        {
            case 0:
                executeLine();
                break;
            case 1:
                executeRectangle();
                break;
            case 2:

                break;
            default:
                break;
        };
    }

    private void executeLine() {
        //The LineRenderer
        LineRenderer lineRenderer;

        // Positions of points to draw in lineRenderer
        List<Vector3> positions = new List<Vector3>();

        // Clear the list of positions
        positions.Clear();


        lineRenderer = brush.createLineRenderer();

        float width = GameObject.Find("Play").GetComponent<Play>().lineWidth;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.startColor = play.currentColor;
        lineRenderer.endColor = play.currentColor;

        // Add a origin point
        positions.Add(brush.transform.position);

        float xTransform = data[0] + brush.transform.position.x;
        float yTransform = data[1] + brush.transform.position.y;

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

        // Misc Debug
        //Debug.Log(new Vector3(xTransform, yTransform, 0f));

        // Add the point from the block to the line renderer
        positions.Add(new Vector3(xTransform, yTransform, 0f));

        brush.transform.position = new Vector3(xTransform, yTransform, 0f);

        // Render lines
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }

    private void executeRectangle()
    {

        // Positions of points to draw in lineRenderer
        List<Vector3> positions = new List<Vector3>();

        // Clear the list of positions
        positions.Clear();


        LineRenderer lineRenderer = brush.createLineRenderer();

        float width = GameObject.Find("Play").GetComponent<Play>().lineWidth;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.startColor = play.currentColor;
        lineRenderer.endColor = play.currentColor;

        // Add a origin point
        positions.Add(brush.transform.position);

        float xTransform = data[0] + brush.transform.position.x;
        float yTransform = data[1] + brush.transform.position.y;

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


        // Add the point from the block to the line renderer
        positions.Add(new Vector3(brush.transform.position.x, yTransform, 0f));
        positions.Add(new Vector3(xTransform, yTransform, 0f));
        positions.Add(new Vector3(xTransform, brush.transform.position.y, 0f));
        positions.Add(new Vector3(brush.transform.position.x, brush.transform.position.y, 0f));
        positions.Add(new Vector3(brush.transform.position.x, yTransform, 0f));

        //positions.Add(new Vector3(-xTransform, 0f, 0f));


        // Render lines
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());

        if (data[2] == 1)
        {
            // Fill
            MeshRenderer meshRenderer = brush.createMeshRenderer();

            Mesh filledMesh = new Mesh();

            // Set the material to use the same color as play.currentColor
            //meshRenderer.material.color = play.currentColor;
            meshRenderer.material.EnableKeyword("_EMISSION");
            meshRenderer.material.SetColor("_EmissionColor", play.currentColor);

            Vector3[] vertices = new Vector3[4];
            int[] triangles = new int[6];

            vertices[0] = positions[0] - brush.transform.position;
            vertices[1] = positions[1] - brush.transform.position;
            vertices[2] = positions[2] - brush.transform.position;
            vertices[3] = positions[3] - brush.transform.position;

            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;

            triangles[3] = 0;
            triangles[4] = 2;
            triangles[5] = 3;

            filledMesh.vertices = vertices;
            filledMesh.triangles = triangles;
            meshRenderer.gameObject.GetComponent<MeshFilter>().mesh = filledMesh;
        }

        brush.transform.position = new Vector3(xTransform, yTransform, 0f);
    }
}
