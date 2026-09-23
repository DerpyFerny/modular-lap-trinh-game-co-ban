using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoUp : MonoBehaviour
{
    public jump playerJump;
    
    void Awake()
    {
        playerJump = GameObject.Find("Player").GetComponent<jump>();
        Debug.Log(playerJump != null);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {  
            playerJump.currGravity = -1f;
            playerJump.rb.gravityScale = playerJump.currGravity;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
         if (collision.CompareTag("Player"))
        {
            playerJump.currGravity = playerJump.defaultGravity;
            playerJump.rb.gravityScale = playerJump.currGravity;
        }
    }
}
