using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRightMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveDistance;
    private float startingYPos;

    void Awake()
    {
        startingYPos = transform.position.y;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time * moveSpeed) * moveDistance + startingYPos, transform.position.z);
    }
}
