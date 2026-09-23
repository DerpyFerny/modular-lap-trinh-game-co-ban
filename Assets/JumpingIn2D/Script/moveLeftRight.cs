using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveLeftRight : MonoBehaviour
{
    public int speed = 10;
    public float smoothValue = 1f;
    public Rigidbody2D rb;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        move();
    }

    void move()
    {
        //Get x (left right)
        //Get y (up down)
        //direction = x,y
        float x_direction = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, x_direction * speed, smoothValue), rb.velocity.y);
    }
}
