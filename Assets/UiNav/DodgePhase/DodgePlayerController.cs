using System.Collections;
using UnityEngine;

/// <summary>
/// Controls the small player character during the Enemy Dodge Phase.
/// Handles 4-directional WASD/Arrow key movement, arena clamping, and damage cooldown.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class DodgePlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Boundary Clamping")]
    [SerializeField] private bool useClamping = true;
    [SerializeField] private Vector2 minBounds = new Vector2(-2.2f, -2.4f);
    [SerializeField] private Vector2 maxBounds = new Vector2(2.2f, 0.4f);

    [Header("Damage & Invulnerability")]
    [SerializeField] private float invulnerabilityDuration = 0.5f;

    [Header("Visual Feedback")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private Unit playerUnit;
    private BattleHUD playerHUD;
    private EnemyDodgePhase dodgePhase;
    private bool isInvulnerable = false;
    private bool canMove = false;
    private Vector3 startLocalPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        startLocalPosition = transform.localPosition;
    }

    /// <summary>
    /// Initializes the dodge player for a new dodge phase.
    /// Resets position, links the existing battle Unit and HUD.
    /// </summary>
    public void Setup(Unit unit, BattleHUD hud, EnemyDodgePhase phase)
    {
        playerUnit = unit;
        playerHUD = hud;
        dodgePhase = phase;

        transform.localPosition = startLocalPosition;
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }

        canMove = true;
        isInvulnerable = false;

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }
    }

    /// <summary>
    /// Disables player control when the dodge phase ends.
    /// </summary>
    public void DisableMovement()
    {
        canMove = false;
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void Update()
    {
        if (!canMove) return;

        // 4-directional movement with WASD or Arrow Keys
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 inputDirection = new Vector2(moveX, moveY).normalized;

        if (rb != null)
        {
            rb.velocity = inputDirection * moveSpeed;
        }

        // Keep player strictly within the dodge arena boundaries
        if (useClamping)
        {
            float clampedX = Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x);
            float clampedY = Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y);
            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }
    }

    /// <summary>
    /// Called when a projectile collides with the dodge player.
    /// Reduces player Unit HP, triggers HUD update, and starts invulnerability cooldown.
    /// </summary>
    public void OnHitByProjectile(int damage)
    {
        if (!canMove || isInvulnerable) return;

        if (playerUnit != null)
        {
            bool isDead = playerUnit.TakeDamage(damage);

            if (playerHUD != null)
            {
                playerHUD.SetHP(playerUnit.currentHP, playerUnit.maxHP);
            }

            if (isDead)
            {
                canMove = false;
                if (dodgePhase != null)
                {
                    dodgePhase.OnPlayerDied();
                }
                return;
            }
        }

        StartCoroutine(InvulnerabilityRoutine());
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;
        float elapsed = 0f;
        float flashInterval = 0.08f;

        while (elapsed < invulnerabilityDuration)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
            }
            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }
        isInvulnerable = false;
    }
}
