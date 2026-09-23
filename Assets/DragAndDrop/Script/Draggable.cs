using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    //Get mouse world position
    Vector3 mousePositionOffset;

    Vector3 GetMouseWorldPosition()
    {
        //camera view -> world view , worl view get mouse position
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    //Drag and drop 
    //On first frame
    void OnMouseDown()
    {
        //Capture mouse offset
        mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();

    }

    //On Drag
    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPosition() + mousePositionOffset;
    }
}
