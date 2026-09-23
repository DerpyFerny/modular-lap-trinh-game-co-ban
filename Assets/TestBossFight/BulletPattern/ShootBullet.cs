using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    //spawn bullet
    //  where ?
    //  what angle to send the bullet ?
    //  how fast does it shoot ?  

    public GameObject Bullet;
    public List<GameObject> Spawnpoint;

    public float fireRate = 1f; 
    public float timer;

    public enum SpawnMode { SingleSpawnpoint, AllSpawnpoints }
    public SpawnMode TypeOfSpawn;

    public enum ShotType {Single , Burst};
    public ShotType TypeOfShot;

    [Header("Burst shot setting")]
    public int bulletPerBurst;
    [Range(0, 359)]public float angleSpread;

    void Update()
    {
        //timer count up, if >= fireRate , shoot -> reset
        timer += Time.deltaTime;
        if(timer >= fireRate)
        {
            switch (TypeOfShot){
                case ShotType.Single:
                    SingleShoot();
                    break;
                case ShotType.Burst:
                    BurstShoot();
                    break;
            }
            timer = 0;    
        }
    }

    void SingleShoot()
    {
        if (TypeOfSpawn == SpawnMode.AllSpawnpoints)
        {
            foreach (GameObject spawn in Spawnpoint)
            {
                Instantiate(Bullet, spawn.transform.position, spawn.transform.rotation);
            }
        }
        else
        {
            GameObject spawn = Spawnpoint[0];
            Instantiate(Bullet, spawn.transform.position, spawn.transform.rotation);
        }
    }

    void BurstShoot()
    {
        if (bulletPerBurst <= 1)
        {
            SingleShoot();
            return;
        }

        float startAngle = -angleSpread / 2f;
        float angleStep = angleSpread / (bulletPerBurst - 1);

        List<GameObject> activeSpawns = TypeOfSpawn == SpawnMode.AllSpawnpoints ? Spawnpoint : new List<GameObject> { Spawnpoint[0] };

        foreach (GameObject spawn in activeSpawns)
        {
            for (int i = 0; i < bulletPerBurst; i++)
            {
                float currentAngle = startAngle + (angleStep * i);
                Quaternion rotation = spawn.transform.rotation * Quaternion.Euler(0, 0, currentAngle);
                Instantiate(Bullet, spawn.transform.position, rotation);
            }
        }
    }
}
