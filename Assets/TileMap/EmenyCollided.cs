using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EmenyCollided : MonoBehaviour
{
    public CharacterStatus playerStatus;
    public Unit playerStats;
    private GameManagerRPG gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").gameObject.GetComponent<GameManagerRPG>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            playerStatus.playerHP = playerStats.currentHP;
            playerStatus.playerX = transform.position.x;
            playerStatus.playerY = transform.position.y;
            playerStatus.enemyID = collision.gameObject.name;

            Debug.Log("Touch enemy");
            gameObject.SetActive(false);
            gameManager.changeScene();
        }
    }
}
