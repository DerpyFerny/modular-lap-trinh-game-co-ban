using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthStatusData", menuName = "StatusObjects/Health", order = 1)]
public class CharacterStatus : ScriptableObject
{
    public float playerHP = 20;
    public float playerX, playerY;
    public string enemyID;
}
