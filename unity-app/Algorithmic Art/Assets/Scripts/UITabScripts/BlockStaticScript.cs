using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles spawning blocks
public class BlockStaticScript : MonoBehaviour
{
    public GameObject prefabToSpawn; // Reference to the prefab you want to spawn
    private GameObject spawnedObject; // Reference to the spawned object
    public Vector3 ScaleObject = new Vector3(1f,1f, 1f);

    private void OnMouseDown()
    {
        // Spawns prefrab when clicked
        SpawnPrefab(transform.position);
    }


    void SpawnPrefab(Vector3 spawnPosition)
    {
        GameObject rootObject = transform.root.gameObject;

        // Get the Canvas component on the GameObject
        Canvas canvas = rootObject.GetComponent<Canvas>();

        // Create the prefab within the same Canvas
        spawnedObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        spawnedObject.transform.localScale = ScaleObject;

        // Sorts out the proper layer and positioning of the new object
        Renderer renderer = spawnedObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.sortingLayerName = "Block";
        }
        spawnedObject.tag = "block";
        spawnedObject.transform.position = new Vector3(spawnedObject.transform.position.x, spawnedObject.transform.position.y, spawnedObject.transform.position.z - 1);
    }

}

