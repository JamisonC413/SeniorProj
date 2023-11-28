using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

// Script for the Draw Block
public class drawBlock : Block
{
    
    // Top snap point
    [SerializeField]
    private GameObject snap1;

    // Bot snap point
    [SerializeField]
    private GameObject snap2;

    // The brushes gameobject 
    public Brush brush;

    // Contains the data that drawBlock needs, uses this and mode to determine the shape and the details of the shape
    public float[] data;

    public float scale = 0.3f;

    private float brushZCoord = 0;
    // Determines the shape that will get drawn
    // 0 = line
    // 1 = rectangle
    public int mode = 0;
    
    // Two main play scripts, the color and linewidth are dictated by these scripts
    public Play play;

    // Sets the starting information for the block, ID, refrences and snap positions
    void Awake()
    {
        initialize();

        snapPositions = new Vector2[2];
        snapPositions[0] = snap1.transform.position;
        snapPositions[1] = snap2.transform.position;

        brush = GameObject.FindGameObjectWithTag("brush").GetComponent<Brush>();
        brushZCoord = brush.transform.position.z;
    }

    // Used to update information on the draw block
    void Update()
    {
        // Updates the snap positions with any new position of block
        snapPositions[0] = snap1.transform.position;
        snapPositions[1] = snap2.transform.position;
    }

    // Initialize basic variables
    public void initialize()
    {
        this.blockID = Block.nextID;
        this.prevBlock = null;
        this.nextBlock = null;

        Block.nextID++;

        play = GameObject.FindGameObjectWithTag("playHandler").GetComponent<Play>();
    }


    // Will be used to draw shape using a child linerenderer component. Not yet implemented
    public override void execute()
    {
        float oldScale = scale;

        if (brush.isMaximized)
        {
            scale *= brush.maximizedScale;
        }

        // Modes for different shapes
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
        scale = oldScale;

    }

    // Responible for drawing a line
    private void executeLine()
    {
        //The LineRenderer
        LineRenderer lineRenderer;

        // Positions of points to draw in lineRenderer
        List<Vector3> positions = new List<Vector3>();

        // Clear the list of positions
        positions.Clear();


        lineRenderer = brush.createLineRenderer();

        float width = play.lineWidth;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.startColor = play.currentColor;
        lineRenderer.endColor = play.currentColor;

        // Add a origin point
        positions.Add(new Vector3(brush.transform.position.x, brush.transform.position.y, brushZCoord));

        float[] scaledData = scaleData(data);

        float xTransform = scaledData[0] + brush.transform.position.x;
        float yTransform = scaledData[1] + brush.transform.position.y;
        Vector2[] drawArea = brush.getDrawArea();
        Debug.Log(drawArea[0]);
        Debug.Log(drawArea[1]);

        // Create bounds for the lines
        if (xTransform < drawArea[0].x)
        {
            xTransform = drawArea[0].x;
        }
        if (yTransform < drawArea[0].y)
        {
            yTransform = drawArea[0].y;
        }
        if (xTransform > drawArea[1].x)
        {
            xTransform = drawArea[1].x;
        }
        if (yTransform > drawArea[1].y)
        {
            yTransform = drawArea[1].y;
        }

        // Misc Debug
        Debug.Log(new Vector3(xTransform, yTransform, brushZCoord));

        // Add the point from the block to the line renderer
        positions.Add(new Vector3(xTransform, yTransform, brushZCoord));

        brush.transform.position = new Vector3(xTransform , yTransform , brush.transform.position.z);

        // Render lines
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }

    // Responible for drawing a rectangle 
    private void executeRectangle()
    {

        // Positions of points to draw in lineRenderer
        List<Vector3> positions = new List<Vector3>();

        // Clear the list of positions
        positions.Clear();


        LineRenderer lineRenderer = brush.createLineRenderer();

        float width = play.lineWidth;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.startColor = play.currentColor;
        lineRenderer.endColor = play.currentColor;

        // Add a origin point
        positions.Add(brush.transform.position);

        float[] scaledData = scaleData(data);

        float xTransform = scaledData[0] + brush.transform.position.x;
        float yTransform = scaledData[1] + brush.transform.position.y;

        Vector2[] drawArea = brush.getDrawArea();
        // Create bounds for the lines
        if (xTransform < drawArea[0].x)
        {
            xTransform = drawArea[0].x;
        }
        if (yTransform < drawArea[0].y)
        {
            yTransform = drawArea[0].y;
        }
        if (xTransform > drawArea[1].x)
        {
            xTransform = drawArea[1].x;
        }
        if (yTransform > drawArea[1].y)
        {
            yTransform = drawArea[1].y;
        }


        // Add the point from the block to the line renderer
        positions.Add(new Vector3(brush.transform.position.x, yTransform, brushZCoord));
        positions.Add(new Vector3(xTransform, yTransform, brushZCoord));
        positions.Add(new Vector3(xTransform, brush.transform.position.y, brushZCoord));
        positions.Add(new Vector3(brush.transform.position.x, brush.transform.position.y, brushZCoord));
        positions.Add(new Vector3(brush.transform.position.x, yTransform, brushZCoord));

        //positions.Add(new Vector3(-xTransform, 0f, 0f));


        // Render lines
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());

        if (data[2] != 0)
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

        brush.transform.position = new Vector3(xTransform , yTransform , brush.transform.position.z);
    }

    private void executeTriangle()
    {
        // Positions of points to draw in lineRenderer
        List<Vector3> positions = new List<Vector3>();

        // Clear the list of positions
        positions.Clear();


        LineRenderer lineRenderer = brush.createLineRenderer();

        float width = play.lineWidth;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.startColor = play.currentColor;
        lineRenderer.endColor = play.currentColor;

        // Add a origin point
        positions.Add(brush.transform.position);

        float[] scaledData = scaleData(data);

        float x1Transform = scaledData[1] / 2 + brush.transform.position.x;
        float x2Transform = scaledData[1] + brush.transform.position.x;
        float yTransform = (float)(scaledData[1] * Math.Sqrt(3) / 2 + brush.transform.position.y);

        Vector2[] drawArea = brush.getDrawArea();
        // Create bounds for the lines

        if (x1Transform > drawArea[1].x)
        {
            x1Transform = drawArea[1].x;
        }
        if (x2Transform > drawArea[1].x)
        {
            x2Transform = drawArea[1].x;
        }
        if (yTransform > drawArea[1].y)
        {
            yTransform = drawArea[1].y;
        }
        // Add the point from the block to the line renderer
        positions.Add(new Vector3(x1Transform, yTransform, brushZCoord));
        positions.Add(new Vector3(x2Transform, brush.transform.position.y, brushZCoord));
        positions.Add(new Vector3(brush.transform.position.x, brush.transform.position.y, brushZCoord));

        //positions.Add(new Vector3(-xTransform, 0f, 0f));


        // Render lines
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());

        if (data[2] != 0)
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
        brush.transform.position = new Vector3(x2Transform, brush.transform.position.y, brush.transform.position.z);
    }

    private void executeCircle()
    {
        // Positions of points to draw in lineRenderer
        List<Vector3> positions = new List<Vector3>();

        var pointCount = 360;

        // Clear the list of positions
        positions.Clear();


        LineRenderer lineRenderer = brush.createLineRenderer();
        float[] scaledData = scaleData(data);

        float width = play.lineWidth;
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

            float x = xScaled * scaledData[1] * 0.5f + brush.transform.position.x;
            float y = yScaled * scaledData[1] * 0.5f + brush.transform.position.y;


            Vector2[] drawArea = brush.getDrawArea();
            // Create bounds for the lines
            //if (x < drawArea[0].x)
            //{
            //    x = drawArea[0].x;
            //}
            //if (y < drawArea[0].y)
            //{
            //    y = drawArea[0].y;
            //}
            //if (x > drawArea[1].x)
            //{
            //    x = drawArea[1].x;
            //}
            //if (y > drawArea[1].y)
            //{
            //    y = drawArea[1].y;
            //}

            positions.Add(new Vector3(x, y, brushZCoord));

        }

        // Render lines
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());

        if (data[2] != 0)
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

                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 2;
                triangles[i * 3 + 2] = i + 1;
            }

            filledMesh.vertices = vertices;
            filledMesh.triangles = triangles;
            meshRenderer.gameObject.GetComponent<MeshFilter>().mesh = filledMesh;
        }

    }

    public float[] scaleData(float[] arr)
    {
        float[] newArr = new float[arr.Length];

        for (int i = 0; i < arr.Length; i++)
        {
            newArr[i] = arr[i] * scale;
        }

        return newArr;
    }
}
