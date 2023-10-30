using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

// Script for the Draw Block
public class drawBlock: Block
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
        snapPositions[0] = snap1.transform.position;
        snapPositions[1] = snap2.transform.position;

        brush = GameObject.Find("Brush").GetComponent<Brush>();
    }

    // Used to update information on the draw block
    void Update()
    {
        // Updates the snap positions with any new position of block
        snapPositions[0] = snap1.transform.position;
        snapPositions[1] = snap2.transform.position;
    }

    public void initialize()
    {
        this.blockID = Block.nextID;
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
                executeTriangle();
                break;
            default:
                executeCircle();
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

    private void executeTriangle()
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

        float x1Transform = (float)data[1]/2 + brush.transform.position.x;
        float x2Transform = data[1] + brush.transform.position.x;
        float yTransform = (float)(data[1] * Math.Sqrt(3)/2 + brush.transform.position.y);

        if (yTransform > brush.startPosition.y + brush.drawArea.y)
        {
            yTransform = brush.startPosition.y + brush.drawArea.y;
        }

        if (x1Transform > brush.startPosition.x + brush.drawArea.x)
        {
            x1Transform = brush.startPosition.x + brush.drawArea.x;
        }

        if (x2Transform > brush.startPosition.x + brush.drawArea.x)
        {
            x2Transform = brush.startPosition.x + brush.drawArea.x;
        }

        // Add the point from the block to the line renderer
        positions.Add(new Vector3(x1Transform, yTransform, 0f));
        positions.Add(new Vector3(x2Transform, brush.transform.position.y, 0f));
        positions.Add(new Vector3(brush.transform.position.x, brush.transform.position.y, 0f));

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

            Vector3[] vertices = new Vector3[3];
            int[] triangles = new int[3];

            vertices[0] = positions[0] - brush.transform.position;
            vertices[1] = positions[1] - brush.transform.position;
            vertices[2] = positions[2] - brush.transform.position;

            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;

            filledMesh.vertices = vertices;
            filledMesh.triangles = triangles;
            meshRenderer.gameObject.GetComponent<MeshFilter>().mesh = filledMesh;
        }

        brush.transform.position = new Vector3(x2Transform, brush.transform.position.y, 0f);
    }

    private void executeCircle()
    {
        // Positions of points to draw in lineRenderer
        List<Vector3> positions = new List<Vector3>();

        var pointCount = 100;

        // Clear the list of positions
        positions.Clear();


        LineRenderer lineRenderer = brush.createLineRenderer();

        float width = GameObject.Find("Play").GetComponent<Play>().lineWidth;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.startColor = play.currentColor;
        lineRenderer.endColor = play.currentColor;

        for (int i = 0; i < pointCount; i++)
        {
            float circumference = (float)i / pointCount;

            float currentRadian = circumference * 2 * Mathf.PI;

            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = xScaled * data[1] + brush.transform.position.x;
            float y = yScaled * data[1] + brush.transform.position.y;

            if (y < brush.startPosition.y)
            {
                y = brush.startPosition.y;
            }

            if (x < brush.startPosition.x)
            {
                x = brush.startPosition.x;
            }

            if (y > brush.startPosition.y + brush.drawArea.y)
            {
                y = brush.startPosition.y + brush.drawArea.y;
            }

            if (x > brush.startPosition.x + brush.drawArea.x)
            {
                x = brush.startPosition.x + brush.drawArea.x;
            }

            positions.Add(new Vector3(x, y, 0f));

        }

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

            Vector3[] vertices = new Vector3[pointCount];
            int[] triangles = new int[3 * pointCount];

            for (int i = 0; i < pointCount; i++)
            {
                vertices[i] = positions[i] - brush.transform.position;
            }

            for (int i = 0; i < pointCount - 2; i++)
            {
          
                triangles[i*3] = 0;
                triangles[i*3 + 1] = i + 2;
                triangles[i*3 + 2] = i + 1;
            }

            filledMesh.vertices = vertices;
            filledMesh.triangles = triangles;
            meshRenderer.gameObject.GetComponent<MeshFilter>().mesh = filledMesh;
        }

    }
}
