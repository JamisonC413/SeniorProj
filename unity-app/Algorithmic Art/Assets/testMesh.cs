using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class SimpleSquare : MonoBehaviour
{
    void Start()
    {
        // Get or create the MeshFilter and MeshRenderer components
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        // Create a new mesh
        Mesh squareMesh = new Mesh();
        squareMesh.name = "SquareMesh";

        // Generate a simple square mesh
        CreateSimpleSquare(squareMesh);

        // Assign the mesh to the MeshFilter
        meshFilter.mesh = squareMesh;

        // Create and assign a material with a solid color to the MeshRenderer
        Material material = new Material(Shader.Find("Standard"));
        material.color = Color.red; // Set the desired color
        meshRenderer.material = material;
    }

    void CreateSimpleSquare(Mesh mesh)
    {
        // Define vertices
        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(-0.5f, -0.5f, 0f);
        vertices[1] = new Vector3(0.5f, -0.5f, 0f);
        vertices[2] = new Vector3(0.5f, 0.5f, 0f);
        vertices[3] = new Vector3(-0.5f, 0.5f, 0f);

        // Define triangles
        int[] triangles = { 0, 1, 2, 0, 2, 3 };

        // Assign the data to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
}
