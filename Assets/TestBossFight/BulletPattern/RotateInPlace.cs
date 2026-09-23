using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateInPlace : MonoBehaviour
{
    public float turnSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f,0f, turnSpeed * Time.deltaTime);
    }
}
