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

    // The brushes gameobject 
    public Brush brush2;

    // Contains the data that drawBlock needs, uses this and mode to determine the shape and the details of the shape
    public float[] data;

    public float scale = 0.3f;

    // Determines the shape that will get drawn
    // 0 = line
    // 1 = rectangle
    public int mode = 0;

    public Play play;
    public Play play2;

    // Sets the starting information for the block, ID, refrences and snap positions
    void Awake()
    {
        initialize();

        snapPositions = new Vector2[2];
        snapPositions[0] = snap1.transform.position;
        snapPositions[1] = snap2.transform.position;

        brush = GameObject.FindGameObjectWithTag("brush").GetComponent<Brush>();
        brush2 = GameObject.FindGameObjectWithTag("brush2").GetComponent<Brush>();
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

        play = GameObject.FindGameObjectWithTag("play").GetComponent<Play>();
    }

    // Will be used to draw line using a child linerenderer component. Not yet implemented
    public override void execute()
    {


        switch (mode)
        {
            case 0:
                executeLine();
                executeLineMaximized();
                break;
            case 1:
                executeRectangle();
                executeRectangleMaximized(brush2);
                break;
            case 2:
                executeTriangle();
                executeTriangleMaximized(brush2);
                break;
            default:
                executeCircle();
                executeCircleMaximized(brush2);
                break;
        };
    }

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
        positions.Add(brush.transform.position);

        float[] scaledData = scaleData(data);
        float xTransform = scaledData[0] + brush.transform.position.x;
        float yTransform = scaledData[1] + brush.transform.position.y;

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

    private void executeLineMaximized()
    {
        //The LineRenderer
        LineRenderer lineRenderer;

        // Positions of points to draw in lineRenderer
        List<Vector3> positions = new List<Vector3>();

        // Clear the list of positions
        positions.Clear();


        lineRenderer = brush2.createLineRenderer();

        float width = play.lineWidth2;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.startColor = play.currentColor;
        lineRenderer.endColor = play.currentColor;

        // Add a origin point
        positions.Add(brush2.transform.position);

        float[] scaledData = scaleData2(data);
        float xTransform = scaledData[0] + brush2.transform.position.x;
        float yTransform = scaledData[1] + brush2.transform.position.y;

        // Create bounds for the lines
        if (xTransform < brush2.startPosition.x)
        {
            xTransform = brush2.startPosition.x;
        }
        if (yTransform < brush2.startPosition.y)
        {
            yTransform = brush2.startPosition.y;
        }
        if (xTransform > brush2.startPosition.x + brush2.drawArea.x)
        {
            xTransform = brush2.startPosition.x + brush2.drawArea.x;
        }
        if (yTransform > brush2.startPosition.y + brush2.drawArea.y)
        {
            yTransform = brush2.startPosition.y + brush2.drawArea.y;
        }

        // Misc Debug
        //Debug.Log(new Vector3(xTransform, yTransform, 0f));

        // Add the point from the block to the line renderer
        positions.Add(new Vector3(xTransform, yTransform, 0f));

        brush2.transform.position = new Vector3(xTransform, yTransform, 0f);

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

        brush.transform.position = new Vector3(xTransform, yTransform, 0f);
    }

    private void executeRectangleMaximized(Brush brush)
    {

        // Positions of points to draw in lineRenderer
        List<Vector3> positions = new List<Vector3>();

        // Clear the list of positions
        positions.Clear();


        LineRenderer lineRenderer = brush.createLineRenderer();

        float width = play.lineWidth2;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.startColor = play.currentColor;
        lineRenderer.endColor = play.currentColor;

        // Add a origin point
        positions.Add(brush.transform.position);

        float[] scaledData = scaleData2(data);

        float xTransform = scaledData[0] + brush.transform.position.x;
        float yTransform = scaledData[1] + brush.transform.position.y;

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

        brush.transform.position = new Vector3(xTransform, yTransform, 0f);
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

        brush.transform.position = new Vector3(x2Transform, brush.transform.position.y, 0f);
    }

    private void executeTriangleMaximized(Brush brush)
    {
        // Positions of points to draw in lineRenderer
        List<Vector3> positions = new List<Vector3>();

        // Clear the list of positions
        positions.Clear();


        LineRenderer lineRenderer = brush.createLineRenderer();

        float width = play.lineWidth2;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.startColor = play.currentColor;
        lineRenderer.endColor = play.currentColor;

        // Add a origin point
        positions.Add(brush.transform.position);

        float[] scaledData = scaleData2(data);

        float x1Transform = scaledData[1] / 2 + brush.transform.position.x;
        float x2Transform = scaledData[1] + brush.transform.position.x;
        float yTransform = (float)(scaledData[1] * Math.Sqrt(3) / 2 + brush.transform.position.y);

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

        brush.transform.position = new Vector3(x2Transform, brush.transform.position.y, 0f);
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

    private void executeCircleMaximized(Brush brush)
    {
        // Positions of points to draw in lineRenderer
        List<Vector3> positions = new List<Vector3>();

        var pointCount = 360;

        // Clear the list of positions
        positions.Clear();


        LineRenderer lineRenderer = brush.createLineRenderer();
        float[] scaledData = scaleData2(data);

        float width = play.lineWidth2;
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

    public float[] scaleData2(float[] arr)
    {
        float[] newArr = new float[arr.Length];

        for (int i = 0; i < arr.Length; i++)
        {
            newArr[i] = arr[i] * 0.55f;
        }

        return newArr;
    }
}
