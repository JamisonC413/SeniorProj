using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startBlock : MonoBehaviour
{

    public GameObject next;
    private Vector3 snapPosition;

    // Start is called before the first frame update
    void Start()
    {
        snapPosition = transform.position + new Vector3(0.0f, -.8f, 0.0f);
        // Debug.Log(snapPosition);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
