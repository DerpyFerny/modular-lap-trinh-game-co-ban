using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToObject : MonoBehaviour
{
    public Vector3 mouse_world_pos;
    void Update()
    {
        mouse_world_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.rotation = Quaternion.LookRotation(Vector3.forward, mouse_world_pos - transform.position);
    }
}
