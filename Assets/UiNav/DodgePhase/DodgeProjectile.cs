using UnityEngine;

/// <summary>
/// Controls a simple straight-moving projectile spawned during the Enemy Dodge Phase.
/// Detects collision with DodgePlayerController, applies damage, and self-destructs.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class DodgeProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float projectileSpeed = 4f;
    [SerializeField] private int projectileDamage = 2;
    [SerializeField] private float lifetime = 5f;

    private Vector2 moveDirection = Vector2.left;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0f;
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.isTrigger = true;
        }

        Destroy(gameObject, lifetime);
    }

    /// <summary>
    /// Configures flight direction, speed, and damage on instantiation.
    /// </summary>
    public void Initialize(Vector2 direction, float speed, int damage)
    {
        moveDirection = direction.normalized;
        projectileSpeed = speed;
        projectileDamage = damage;

        if (rb != null)
        {
            rb.velocity = moveDirection * projectileSpeed;
        }
    }

    private void Update()
    {
        // Fallback translation if no Rigidbody2D or Rigidbody2D is Kinematic
        if (rb == null || rb.isKinematic)
        {
            transform.position += (Vector3)(moveDirection * projectileSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DodgePlayerController player = collision.GetComponent<DodgePlayerController>();
        if (player != null)
        {
            player.OnHitByProjectile(projectileDamage);
            Destroy(gameObject);
        }
    }
}
