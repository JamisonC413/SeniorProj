using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour
{
    // Tracks the hieght and width of the rectangular draw area. Used to 
    // contain the brush to a specific area
    public Vector2 drawArea = new Vector2(10f, 10f);

    // The starting position of the brush (to reset between hitting play)
    private Vector3 startBrush;

    // Start is called before the first frame update
    void Start()
    {
        // Save the brush location
        startBrush = gameObject.transform.position;
    }

    public void resetPosition()
    {
        // Start brush at the start position
        gameObject.transform.position = startBrush;
    }
}
