using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DeleteAll : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void deleteAllBlocks()
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("block");

        foreach (GameObject block in blocks)
        {
            Destroy(block);
        }
    }
}
