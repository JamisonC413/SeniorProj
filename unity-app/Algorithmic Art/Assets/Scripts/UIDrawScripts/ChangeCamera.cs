using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    // Main camera, which is the camera for the minimized view
    public GameObject mainCamera;

    // Second camera, which is the camera for the maximized view
    public GameObject camera2;

    // Canvas of maximized view
    public GameObject canvas;

    // Boolean fields to determine the state of the view
    public static bool maximized = false;
    public static bool minimized = true;

    // Swap to maximized view
    public void maximize()
    {
        camera2.SetActive(true);
        canvas.SetActive(true);
        mainCamera.SetActive(false);

        MoveAllBlocks.moveAllBlocks();

        maximized = true;
        minimized = false;
    }

    // Swap to minimzed view
    public void minimize()
    {
        canvas.SetActive(false);
        mainCamera.SetActive(true);
        camera2.SetActive(false);

        MoveAllBlocks.moveAllBlocks();

        maximized = false;
        minimized = true;

    }
}
