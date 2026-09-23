using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "New Attack", menuName = "Boss/Attack Setting")]
public class AttackSettingSO : ScriptableObject
{
    public GameObject bullet;
    public Bullet.PatternType typeOfPattern;
    public float bulletSpeed;
    public float bulletTimer;

    [ShowIf("typeOfPattern", Bullet.PatternType.Sine)] public float bulletSineSpeed;
    [ShowIf("typeOfPattern", Bullet.PatternType.Sine)] public float bulletSineAmplitude;
    [ShowIf("typeOfPattern", Bullet.PatternType.Circular)] public float bulletRadius;

    public bool isRotatingToObject = false;
    public bool isRotatingInPlace = false;
    public ShootBullet.ShotType typeOfShot;
    public ShootBullet.SpawnMode spawnMode;
    [ShowIf("isRotatingInPlace")] public int turnSpeed;
    [ShowIf("typeOfShot", ShootBullet.ShotType.Burst)] public int bulletPerBurst;
    [ShowIf("typeOfShot", ShootBullet.ShotType.Burst)][Range(0, 359)] public float angleSpread;
    public float fireRate;
    public float attackTime;
}
