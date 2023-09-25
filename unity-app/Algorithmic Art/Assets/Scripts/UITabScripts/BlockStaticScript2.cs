using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStaticScript2 : MonoBehaviour
{
    // Start is called before the first frame update
    //public GameObject blockDrag;
    public GameObject prefabToSpawn; // Reference to the prefab you want to spawn
    private GameObject spawnedObject; // Reference to the spawned object
    private void OnMouseDown()
    {
        SpawnPrefab(transform.position);

    }


    void SpawnPrefab(Vector3 spawnPosition)
    {

        GameObject rootObject = transform.root.gameObject;

        // Get the Canvas component on the GameObject
        Canvas canvas = rootObject.GetComponent<Canvas>();

        // Create the prefab within the same Canvas
        spawnedObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity, canvas.transform);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

