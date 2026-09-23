using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jump : MonoBehaviour
{
    
    public float jumpPower = 10;
    public Vector2 groundCheckPosition;
    public Vector2 groundCheckSize;
    public float groundCheckLength = 1f;
    public bool canJump = true;
    public Rigidbody2D rb;
    public float currGravity;
    public float defaultGravity;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        rb = gameObject.GetComponent<Rigidbody2D>();
        defaultGravity = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();    
        }
        if(isGround())
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }
    }
    void Jump()
    {
        //if(found ground)
        //  can jump
        //else cannot jump

        if(isGround())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }
    }

    bool isGround()
    {
        return Physics2D.BoxCast((Vector2)transform.position + groundCheckPosition , groundCheckSize , 0 , Vector2.down, groundCheckLength, LayerMask.GetMask("ground"));
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)transform.position + groundCheckPosition , groundCheckSize);
    }
}
