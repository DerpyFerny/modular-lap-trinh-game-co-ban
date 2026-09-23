using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class EDmovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        EightDirectionMove();
    }

    void EightDirectionMove()
    {
        float x_direction = Input.GetAxisRaw("Horizontal");
        float y_direction = Input.GetAxisRaw("Vertical");

        Vector2 moveDirection = new Vector2(x_direction,y_direction);

        rb.velocity = moveDirection.normalized * speed;
    }
}
