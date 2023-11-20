using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHandler : MonoBehaviour
{

    public GameObject grid;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // temp grid function for experimental purposes
    public void toggleGrid()
    {
        if(grid.activeSelf)
        {
            grid.SetActive(false);
        }
        else
        {
            grid.SetActive(true);
        }
    }
}
