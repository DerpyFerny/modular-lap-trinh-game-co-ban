using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ToNextRoom : MonoBehaviour
{
    [SerializeField] private SceneAsset nextRoom;
    private Transform player;
    public Vector2 newRoomPlayerPosition;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")){
            player = collision.transform;
            player.position = newRoomPlayerPosition;
            SceneManager.LoadScene(nextRoom.name);
        }
    }
}
