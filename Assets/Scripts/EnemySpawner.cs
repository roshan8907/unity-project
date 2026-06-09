using UnityEngine;

/// <summary>
/// Handles the periodic spawning of enemies within a specified area.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    /// <summary>
    /// The prefab used to instantiate enemies.
    /// </summary>
    public GameObject enemyPrefab;

    /// <summary>
    /// The interval in seconds between each enemy spawn.
    /// </summary>
    public float spawnTime = 5f;

    /// <summary>
    /// Starts the repeating invocation of the SpawnEnemy method.
    /// </summary>
    void Start()
    {
        InvokeRepeating("SpawnEnemy", 2f, spawnTime);
    }

    /// <summary>
    /// Spawns an enemy at a random position within the defined boundaries.
    /// </summary>
    void SpawnEnemy()
    {
        Vector2 spawnPosition = new Vector2(
            Random.Range(-3f, 3f),
            Random.Range(-3f, 3f)
        );

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}