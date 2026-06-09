using UnityEngine;

/// <summary>
/// Handles dealing damage to the player when the enemy comes into contact with them.
/// </summary>
public class EnemyDamage : MonoBehaviour
{
    /// <summary>
    /// The amount of damage dealt to the player per hit.
    /// </summary>
    [Header("Damage Settings")]
    public float damage = 10f;

    /// <summary>
    /// The minimum time in seconds between consecutive damage ticks while still in contact.
    /// </summary>
    public float damageCooldown = 1.0f;

    private float lastDamageTime;
    private PlayerHealth contactedPlayer;

    /// <summary>
    /// Initializes the cooldown timer to allow immediate damage upon first contact.
    /// </summary>
    void Start()
    {
        // Set lastDamageTime so damage can be dealt immediately on first contact
        lastDamageTime = -damageCooldown;
        Debug.Log($"[EnemyDamage] Start: Initialized on {gameObject.name}. Damage: {damage}, Cooldown: {damageCooldown}");
    }

    /// <summary>
    /// Continuously checks and deals damage if the player remains in contact after the cooldown.
    /// </summary>
    void Update()
    {
        // Drive repeat damage from Update() so Rigidbody2D sleep never blocks it
        if (contactedPlayer != null && Time.time >= lastDamageTime + damageCooldown)
        {
            contactedPlayer.TakeDamage(damage);
            lastDamageTime = Time.time;
            Debug.Log($"[EnemyDamage] (Update) Dealt {damage} damage to {contactedPlayer.gameObject.name}.");
        }
    }

    // --- Collision callbacks ---

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"[EnemyDamage] OnCollisionEnter2D on {gameObject.name} with {collision.gameObject.name} (Tag: {collision.gameObject.tag})");
        SetContact(collision.gameObject);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        ClearContact(collision.gameObject);
    }

    // --- Trigger callbacks ---

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[EnemyDamage] OnTriggerEnter2D on {gameObject.name} with {other.gameObject.name} (Tag: {other.gameObject.tag})");
        SetContact(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ClearContact(other.gameObject);
    }

    // --- Helpers ---

    /// <summary>
    /// Sets the currently contacted player and deals an initial instance of damage if the cooldown has elapsed.
    /// </summary>
    /// <param name="target">The target GameObject involved in the collision or trigger event.</param>
    private void SetContact(GameObject target)
    {
        PlayerHealth ph = FindPlayerHealth(target);
        if (ph != null)
        {
            contactedPlayer = ph;

            // Deal first hit immediately (respects cooldown)
            if (Time.time >= lastDamageTime + damageCooldown)
            {
                contactedPlayer.TakeDamage(damage);
                lastDamageTime = Time.time;
                Debug.Log($"[EnemyDamage] First-contact dealt {damage} damage to {ph.gameObject.name}.");
            }
        }
    }

    /// <summary>
    /// Clears the currently contacted player, stopping continuous damage.
    /// </summary>
    /// <param name="target">The target GameObject that has ended contact.</param>
    private void ClearContact(GameObject target)
    {
        PlayerHealth ph = FindPlayerHealth(target);
        if (ph != null && ph == contactedPlayer)
        {
            contactedPlayer = null;
            Debug.Log($"[EnemyDamage] Contact ended with {ph.gameObject.name}.");
        }
    }

    /// <summary>
    /// Safely attempts to find the PlayerHealth component on the given target or its parents/children.
    /// </summary>
    /// <param name="target">The target GameObject to check for PlayerHealth.</param>
    /// <returns>The found PlayerHealth component, or null if none exists.</returns>
    private PlayerHealth FindPlayerHealth(GameObject target)
    {
        if (!target.CompareTag("Player") && target.GetComponentInParent<PlayerHealth>() == null)
        {
            return null;
        }

        PlayerHealth ph = target.GetComponent<PlayerHealth>();
        if (ph == null)
        {
            ph = target.GetComponentInParent<PlayerHealth>();
        }
        if (ph == null)
        {
            ph = target.GetComponentInChildren<PlayerHealth>();
        }
        return ph;
    }
}