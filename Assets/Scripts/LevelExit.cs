using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Monitors the scene for enemy presence and loads the win scene when all enemies are defeated.
/// </summary>
public class LevelExit : MonoBehaviour
{
    /// <summary>
    /// Continuously checks if any enemies remain in the scene.
    /// Transitions to the WinScene if no enemies are found.
    /// </summary>
    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
        {
            SceneManager.LoadScene("WinScene");
        }
    }
}