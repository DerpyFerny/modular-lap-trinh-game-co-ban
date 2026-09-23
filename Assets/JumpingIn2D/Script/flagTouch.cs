using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flagTouch : MonoBehaviour
{
    private game_manager gameManager;
    void Awake()
    {
        gameManager = GameObject.Find("GM").GetComponent<game_manager>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("touch flag, going to next lvl");
            gameManager.currLvl++;
            gameManager.toNextLevel();
        }
    }
}
