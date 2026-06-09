using UnityEngine;

/// <summary>
/// Controls the movement logic for enemies, making them follow the player.
/// </summary>
public class EnemyAI : MonoBehaviour
{
    /// <summary>
    /// The base speed at which the enemy moves toward the player.
    /// </summary>
    public float speed = 2f;

    /// <summary>
    /// The distance at which the enemy will stop moving toward the player to avoid overlapping.
    /// </summary>
    public float stoppingDistance = 0.7f;

    private Transform player;
    private float currentSpeed;

    /// <summary>
    /// Locates the player and assigns a randomized speed variation to the enemy.
    /// </summary>
    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        // Add speed variation: +/- 0.4f from the base speed
        currentSpeed = speed + Random.Range(-0.4f, 0.4f);
    }

    /// <summary>
    /// Moves the enemy toward the player every frame, stopping once within stoppingDistance.
    /// </summary>
    void Update()
    {
        if (player == null)
        {
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        // Only move toward the player if we are outside the stopping distance
        if (distance > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                currentSpeed * Time.deltaTime
            );
        }
    }
}