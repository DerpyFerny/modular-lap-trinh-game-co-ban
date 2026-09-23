using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class BossManager : MonoBehaviour
{
    public List<GameObject> AllBulletSpawner;

    [System.Serializable]
    public class AttackEntry
    {
        public AttackSettingSO attackSetting;
        public List<GameObject> bulletSpawner;
    }
    [Header("Attacks")]
    [SerializeField] private List<AttackEntry> attacks;
    [SerializeField] private int attackIndex = 0;
    public bool Attacking = false;

    void Update()
    {
        if (!Attacking)
        {
            foreach (GameObject spawn in AllBulletSpawner)
            {
                spawn.SetActive(false);
            }
        }
        else
        {
            if (attackIndex < 0 || attackIndex >= attacks.Count)
            {
                Debug.LogWarning($"BossManager: attackIndex {attackIndex} is out of range.");
                return;
            }
            AttackEntry entry = attacks[attackIndex];
            AttackPattern(entry.attackSetting,entry.bulletSpawner);
            StartCoroutine(AttackEnd(entry.attackSetting.attackTime));
        }
    }

    public void SetAttackingTrue()
    {
        Attacking = true;
    }

    void AttackPattern(AttackSettingSO attackSetting, List<GameObject> bulletSpawner)
    {
        Bullet bullet = attackSetting.bullet.GetComponent<Bullet>();
        bullet.TypeOfPattern = attackSetting.typeOfPattern;
        bullet.BulletSpeed = attackSetting.bulletSpeed;
        bullet.BulletTimer = attackSetting.bulletTimer;
        bullet.sineAmplitude = attackSetting.bulletSineAmplitude;
        bullet.sineSpeed = attackSetting.bulletSineSpeed;
        bullet.radius = attackSetting.bulletRadius;
        foreach (GameObject Spawner in bulletSpawner)
        {
            Spawner.SetActive(true);
            RotateToObject rto = Spawner.GetComponent<RotateToObject>();
            RotateInPlace rip = Spawner.GetComponent<RotateInPlace>();
            ShootBullet sb = Spawner.GetComponent<ShootBullet>();
            rto.enabled = attackSetting.isRotatingToObject;
            rip.turnSpeed = attackSetting.turnSpeed;
            rip.enabled = attackSetting.isRotatingInPlace;
            sb.Bullet = attackSetting.bullet;
            sb.TypeOfSpawn = attackSetting.spawnMode;
            sb.TypeOfShot = attackSetting.typeOfShot;
            sb.bulletPerBurst = attackSetting.bulletPerBurst;
            sb.angleSpread = attackSetting.angleSpread;
            sb.fireRate = attackSetting.fireRate;
        }
    }

    IEnumerator AttackEnd(float timer)
    {
        yield return new WaitForSecondsRealtime(timer);

        Attacking = false;
    }
}
