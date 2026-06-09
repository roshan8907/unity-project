using UnityEngine;

/// <summary>
/// Controls the movement and collision behavior of bullets fired by the player.
/// Automatically targets the nearest enemy or boss upon spawning.
/// </summary>
public class Bullet : MonoBehaviour
{
    /// <summary>
    /// The travel speed of the bullet.
    /// </summary>
    public float speed = 5f;

    /// <summary>
    /// The amount of damage dealt by the bullet upon hitting an enemy or boss.
    /// </summary>
    public int damage = 10;

    private Vector2 moveDirection = Vector2.right;

    /// <summary>
    /// Initializes bullet colliders and determines the target direction.
    /// </summary>
    void Start()
    {
        // Force all colliders on the bullet to be triggers programmatically
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.isTrigger = true;
        }

        // Find nearest enemy or boss to aim at
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");
        
        GameObject target = null;
        float minDistance = float.MaxValue;
        
        // Check standard enemies
        if (enemies != null)
        {
            foreach (GameObject go in enemies)
            {
                if (go == null || !go.activeInHierarchy) continue;
                float dist = Vector2.Distance(transform.position, go.transform.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    target = go;
                }
            }
        }
        
        // Check bosses
        if (bosses != null)
        {
            foreach (GameObject go in bosses)
            {
                if (go == null || !go.activeInHierarchy) continue;
                float dist = Vector2.Distance(transform.position, go.transform.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    target = go;
                }
            }
        }

        if (target != null)
        {
            // Aim at the closest target
            moveDirection = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        else
        {
            // Fallback to player facing direction if no targets are found
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerMovement pm = player.GetComponent<PlayerMovement>();
                float direction = 1f;
                
                if (pm != null)
                {
                    direction = pm.facingDirection;
                }
                else
                {
                    direction = player.transform.localScale.x > 0f ? 1f : -1f;
                }

                moveDirection = new Vector2(direction, 0f);

                if (direction < 0f)
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                }
                else
                {
                    transform.rotation = Quaternion.identity;
                }
            }
            else
            {
                moveDirection = transform.right;
            }
        }
    }

    /// <summary>
    /// Moves the bullet steadily in the specified direction each frame.
    /// </summary>
    void Update()
    {
        // Move in world space to be completely immune to local rotation overrides
        transform.position += (Vector3)(moveDirection * speed * Time.deltaTime);
    }

    /// <summary>
    /// Handles collision detection with enemies, the player, and environmental objects.
    /// </summary>
    /// <param name="other">The collider the bullet has hit.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        // Ignore collisions with Player
        if (other.CompareTag("Player"))
        {
            return;
        }

        // Ignore collisions with other bullets
        if (other.GetComponent<Bullet>() != null)
        {
            return;
        }

        // Deal damage to enemies or boss
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // Ignore triggers (e.g., health pickups, teleport holes)
        if (other.isTrigger)
        {
            return;
        }

        // Destroy self on solid obstacles (walls, etc.)
        Destroy(gameObject);
    }
}