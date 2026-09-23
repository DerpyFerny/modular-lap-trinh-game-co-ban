using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerRPG : MonoBehaviour
{
    public SceneAsset BattleScene;
    public GameObject[] inactiveGameObject;
    private CharacterStatus playerData;
    private string currentScene;

    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        playerData = GameObject.Find("Player").GetComponent<EmenyCollided>().playerStatus;
    }
    public void changeScene()
    {
        foreach(GameObject obj in inactiveGameObject)
        {
            obj.SetActive(false);
        }
        SceneManager.LoadScene(BattleScene.name);
    }
    
    public void changeSceneFromBattle(Unit unit)
    {
        playerData.playerHP = unit.currentHP;
        SceneManager.LoadScene(currentScene);
    }
}
