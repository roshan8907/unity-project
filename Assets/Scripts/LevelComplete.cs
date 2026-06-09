using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Monitors the current level state and transitions to the next level when all enemies are defeated.
/// </summary>
public class LevelComplete : MonoBehaviour
{
    /// <summary>
    /// The name of the next scene to load upon completing the current level.
    /// </summary>
    public string nextLevelName = "Level2";

    /// <summary>
    /// Continuously checks if all enemies have been destroyed to trigger the level transition.
    /// </summary>
    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
        {
            Debug.Log("Welcome to Level 2!");
            SceneManager.LoadScene(nextLevelName);
        }
    }
}