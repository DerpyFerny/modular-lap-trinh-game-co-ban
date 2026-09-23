using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cookie : MonoBehaviour
{
    public UnityEvent addPointFromCursor;
    //On click -> +1 point
    void OnMouseDown()
    {
        addPointFromCursor.Invoke();
    }
}
