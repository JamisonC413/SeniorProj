using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorAndRotateCube : MonoBehaviour
{
    // Rotation speed. Can be updated from the browser.
    float rotationSpeed = 50f;

    // Reference to the euler angles of this cube that will be updated each frame.
    Vector3 angles;

    // Start is called before the first frame update
    void Start()
    {
        // Set the initial euler angles of this cube to a random rotation amount.
        angles = Vector3.one * Random.Range(0f, 50f);

        // Give this cube a random color.
        this.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        // Animate this cube's rotation by changing its euler angles given time and rotation speed.
        angles += Vector3.one * Time.deltaTime * rotationSpeed;
        this.transform.eulerAngles = angles;
    }

    // Function to update the rotation speed. Called from the browser.
    public void updateRotationSpeed(int newSpeed)
    {
        rotationSpeed = (float) newSpeed;
    }
}
