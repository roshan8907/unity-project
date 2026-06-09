using UnityEngine;

/// <summary>
/// Defines a collectible item that heals the player and relocates itself periodically.
/// </summary>
public class HealthPickup : MonoBehaviour
{
    /// <summary>
    /// The amount of health to restore when the player collects this pickup.
    /// </summary>
    public float healAmount = 50f;

    /// <summary>
    /// The minimum X boundary for random relocation.
    /// </summary>
    public float minX = -6f;

    /// <summary>
    /// The maximum X boundary for random relocation.
    /// </summary>
    public float maxX = 6f;

    /// <summary>
    /// The minimum Y boundary for random relocation.
    /// </summary>
    public float minY = -3f;

    /// <summary>
    /// The maximum Y boundary for random relocation.
    /// </summary>
    public float maxY = 3f;

    private float timer;

    /// <summary>
    /// Positions the pickup at a random location on start.
    /// </summary>
    void Start()
    {
        MoveHeart();
    }

    /// <summary>
    /// Updates the timer and moves the pickup every 10 seconds.
    /// </summary>
    void Update()
    {
        timer += Time.deltaTime;

        // Move every 10 seconds
        if (timer >= 10f)
        {
            MoveHeart();
            timer = 0f;
        }
    }

    /// <summary>
    /// Detects when the player overlaps with the pickup and applies healing.
    /// </summary>
    /// <param name="collision">The collider that triggered the event.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);

                Debug.Log("Player healed +50!");

                // Move heart instead of destroying
                MoveHeart();
            }
        }
    }

    /// <summary>
    /// Relocates the pickup to a random position within the defined boundaries.
    /// </summary>
    void MoveHeart()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        transform.position = new Vector3(randomX, randomY, 0f);
    }
}