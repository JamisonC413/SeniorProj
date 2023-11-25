using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCamera : MonoBehaviour
{
    // Main camera, which is the camera for the minimized view
    public GameObject mainCamera;

    // Second camera, which is the camera for the maximized view
    public GameObject camera2;

    // Canvas of minimized view
    public GameObject canvasMinView;

    // Canvas of maximized view
    public GameObject canvasMaxView;

    public ScrollbarSync scrollbarSync;

    public Brush brush;

    // Boolean fields to determine the state of the view
    public static bool maximized = false;
    public static bool minimized = true;

    // Swap to maximized view
    public void maximize()
    {
        mainCamera.SetActive(false);
        canvasMinView.SetActive(false);
        canvasMaxView.SetActive(true);
        camera2.SetActive(true);

        MoveAllBlocks.moveAllBlocks();

        scrollbarSync.SyncScrollRectsMax();
        brush.moveDrawing();

        maximized = true;
        minimized = false;
    }

    // Swap to minimzed view
    public void minimize()
    {
        canvasMaxView.SetActive(false);
        camera2.SetActive(false);
        canvasMinView.SetActive(true);
        mainCamera.SetActive(true);
        

        MoveAllBlocks.moveAllBlocks();

        scrollbarSync.SyncScrollRectsMin();
        brush.moveDrawing();

        maximized = false;
        minimized = true;

    }
}
