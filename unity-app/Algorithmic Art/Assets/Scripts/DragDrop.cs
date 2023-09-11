using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop3 : MonoBehaviour
{
    Vector3 offset;

    private void OnMouseDown()
    {
        offset = transform.position - MouseWorldPos();
    }

    private void OnMouseDrag()
    {
        transform.position = MouseWorldPos() + offset;
    }


    Vector3 MouseWorldPos()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }
}
