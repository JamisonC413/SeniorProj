using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionSpawner : BlockSpawner
{
    //Lock tracking if another function exists
    public bool isLocked;

    // Start is called before the first frame update
    void Start()
    {
        isLocked = false;
        playScript = GameObject.FindGameObjectWithTag("playHandler").GetComponent<Play>();
    }

    public override void OnMouseDown()
    {
        //Check that there isn't already a funciton block and that play isn't running
        if (!isLocked && !playScript.locked) {
            base.SpawnPrefab(transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
