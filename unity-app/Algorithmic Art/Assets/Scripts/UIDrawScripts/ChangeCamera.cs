using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    public GameObject mainCamera;

    public GameObject camera2;
    public GameObject canvas;


    public void maximize()
    {
        camera2.SetActive(true);
        canvas.SetActive(true);
        mainCamera.SetActive(false);
        
    }

    public void minimize()
    {
        canvas.SetActive(false);
        mainCamera.SetActive(true);
        camera2.SetActive(false);
        

    }
}
