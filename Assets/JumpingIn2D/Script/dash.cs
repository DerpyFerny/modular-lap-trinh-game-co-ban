using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class dash : MonoBehaviour
{
    // Start is called before the first frame update
    
    public Rigidbody2D rb;
    public float dashPower;
    public bool canDash = true;
    public Vector2 dashDirection;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        dashDirection = new Vector2 (Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if (Input.GetKeyDown(KeyCode.Z) && canDash)
        {
            Dash();
        }
    }

    void Dash()
    {
        //dash happen here
        canDash = false;
        rb.velocity = new Vector2(dashDirection.x * dashPower , dashDirection.y * dashPower);
        canDash = true;
    }
}
