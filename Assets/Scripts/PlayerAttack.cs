using UnityEngine;

/// <summary>
/// Handles player attacks, including melee sword swings and ranged gun shots.
/// </summary>
public class PlayerAttack : MonoBehaviour
{
    /// <summary>
    /// The damage dealt by the sword attack.
    /// </summary>
    [Header("Sword")]
    public int damage = 10;

    /// <summary>
    /// The radius of the sword attack area.
    /// </summary>
    public float attackRange = 1f;

    /// <summary>
    /// The physics layer that identifies targetable enemies.
    /// </summary>
    public LayerMask enemyLayer;

    /// <summary>
    /// The center point from which the sword attack radius is calculated.
    /// </summary>
    public Transform attackPoint;

    /// <summary>
    /// The audio source that plays the sword swing sound.
    /// </summary>
    public AudioSource swordAudio;

    /// <summary>
    /// Indicates whether the player has unlocked the gun weapon.
    /// </summary>
    [Header("Gun")]
    public bool hasGun = false;

    /// <summary>
    /// The projectile prefab to instantiate when firing the gun.
    /// </summary>
    public GameObject bulletPrefab;

    /// <summary>
    /// The transform representing the muzzle of the gun where bullets spawn.
    /// </summary>
    public Transform firePoint;

    /// <summary>
    /// Checks for attack input each frame.
    /// </summary>
    void Update()
    {
        // Sword attack using the Spacebar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }

        // Gun shooting using the Left Mouse Button (if unlocked)
        if (hasGun && Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    /// <summary>
    /// Performs a melee attack, dealing damage to any enemies within the attack range.
    /// </summary>
    void Attack()
    {
        if (swordAudio != null)
        {
            swordAudio.Play();
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayer
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
    }

    /// <summary>
    /// Fires a projectile from the fire point if the gun is unlocked.
    /// </summary>
    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.Log("Bullet Prefab or FirePoint missing!");
            return;
        }

        Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation
        );
    }

    /// <summary>
    /// Draws the attack range in the editor for debugging purposes.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            attackPoint.position,
            attackRange
        );
    }
}