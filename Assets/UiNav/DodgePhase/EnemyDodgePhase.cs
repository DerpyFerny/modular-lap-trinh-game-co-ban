using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Coordinates the Enemy Dodge Phase during ENEMY_TURN.
/// Manages arena visibility, projectile spawning, phase timer, and clean up.
/// </summary>
public class EnemyDodgePhase : MonoBehaviour
{
    [Header("Arena & Player References")]
    [SerializeField] private GameObject arenaContainer;
    [SerializeField] private DodgePlayerController dodgePlayer;

    [Header("Projectile Spawning")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 0.8f;
    [SerializeField] private float projectileSpeed = 4f;
    [SerializeField] private int projectileDamage = 2;

    [Header("Dodge Phase Settings")]
    [SerializeField] private float dodgePhaseDuration = 5f;

    /// <summary>
    /// True while the dodge phase is actively running.
    /// </summary>
    public bool IsPhaseActive { get; private set; } = false;

    private List<GameObject> spawnedProjectiles = new List<GameObject>();
    private Coroutine spawnRoutine;
    private Coroutine timerRoutine;

    private void Awake()
    {
        // Ensure the arena is hidden when battle begins
        if (arenaContainer != null)
        {
            arenaContainer.SetActive(false);
        }
    }

    /// <summary>
    /// Starts the dodge phase: opens arena, enables player, and begins projectile waves.
    /// </summary>
    public void StartDodgePhase(Unit playerUnit, BattleHUD playerHUD)
    {
        IsPhaseActive = true;
        ClearProjectiles();

        if (arenaContainer != null)
        {
            arenaContainer.SetActive(true);
        }

        if (dodgePlayer != null)
        {
            dodgePlayer.Setup(playerUnit, playerHUD, this);
        }

        spawnRoutine = StartCoroutine(SpawnRoutine());
        timerRoutine = StartCoroutine(TimerRoutine());
    }

    /// <summary>
    /// Ends the dodge phase: stops spawning, removes remaining projectiles, and hides arena.
    /// </summary>
    public void StopDodgePhase()
    {
        if (!IsPhaseActive) return;
        IsPhaseActive = false;

        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }

        if (timerRoutine != null)
        {
            StopCoroutine(timerRoutine);
            timerRoutine = null;
        }

        if (dodgePlayer != null)
        {
            dodgePlayer.DisableMovement();
        }

        ClearProjectiles();

        if (arenaContainer != null)
        {
            arenaContainer.SetActive(false);
        }
    }

    /// <summary>
    /// Called immediately if player HP drops to 0 during the dodge phase.
    /// </summary>
    public void OnPlayerDied()
    {
        StopDodgePhase();
    }

    private void ClearProjectiles()
    {
        for (int i = spawnedProjectiles.Count - 1; i >= 0; i--)
        {
            if (spawnedProjectiles[i] != null)
            {
                Destroy(spawnedProjectiles[i]);
            }
        }
        spawnedProjectiles.Clear();
    }

    private IEnumerator TimerRoutine()
    {
        yield return new WaitForSeconds(dodgePhaseDuration);
        StopDodgePhase();
    }

    private IEnumerator SpawnRoutine()
    {
        // Brief initial delay so the player can see the arena before bullets fly
        yield return new WaitForSeconds(0.4f);

        while (IsPhaseActive)
        {
            SpawnProjectile();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnProjectile()
    {
        if (projectilePrefab == null) return;

        Vector3 spawnPos;
        Vector2 travelDirection;

        // If explicit spawn points are assigned, choose one and fire towards arena center
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
            spawnPos = sp.position;
            Vector3 target = arenaContainer != null ? arenaContainer.transform.position : transform.position;
            travelDirection = ((Vector2)(target - spawnPos)).normalized;
        }
        else
        {
            // Default 4-way perimeter spawning around arenaContainer center
            Vector3 center = arenaContainer != null ? arenaContainer.transform.position : transform.position;
            int side = Random.Range(0, 4);
            switch (side)
            {
                case 0: // Left -> moving Right
                    spawnPos = center + new Vector3(-3.2f, Random.Range(-1.2f, 1.2f), 0f);
                    travelDirection = Vector2.right;
                    break;
                case 1: // Right -> moving Left
                    spawnPos = center + new Vector3(3.2f, Random.Range(-1.2f, 1.2f), 0f);
                    travelDirection = Vector2.left;
                    break;
                case 2: // Top -> moving Down
                    spawnPos = center + new Vector3(Random.Range(-2f, 2f), 2f, 0f);
                    travelDirection = Vector2.down;
                    break;
                default: // Bottom -> moving Up
                    spawnPos = center + new Vector3(Random.Range(-2f, 2f), -2f, 0f);
                    travelDirection = Vector2.up;
                    break;
            }
        }

        GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        spawnedProjectiles.Add(proj);

        DodgeProjectile dp = proj.GetComponent<DodgeProjectile>();
        if (dp != null)
        {
            dp.Initialize(travelDirection, projectileSpeed, projectileDamage);
        }
    }
}
