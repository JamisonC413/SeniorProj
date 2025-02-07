using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Handles spawning blocks
public class BlockSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn; // Reference to the prefab you want to spawn
    private GameObject spawnedObject; // Reference to the spawned object
    public Vector3 ScaleObject = new Vector3(1f,1f, 1f);

    //Play Script for locking movement
    public Play playScript;

    private void Start()
    {
        playScript = GameObject.FindGameObjectWithTag("playHandler").GetComponent<Play>();
    }

    public virtual void OnMouseDown()
    {
        //Make sure play isn't running
        if (!playScript.locked)
        {
        // Spawns prefrab when clicked
        SpawnPrefab(transform.position);
        }
    }


    public void SpawnPrefab(Vector3 spawnPosition)
    {
        GameObject rootObject = transform.root.gameObject;

        // Get the Canvas component on the GameObject
        Canvas canvas = rootObject.GetComponent<Canvas>();

        // Create the prefab within the same Canvas
        spawnedObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        //Vector3 scale = new Vector3(33, 28f, 0);
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

