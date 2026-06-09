using UnityEngine;

/// <summary>
/// Attach this to any enemy prefab.
/// When the enemy is destroyed, it adds 1 point to ScoreManager.score.
/// Does NOT touch EnemyHealth, Bullet, or any other existing script.
/// </summary>
public class EnemyScoreReward : MonoBehaviour
{
    /// <summary>
    /// Called when the GameObject is destroyed. Awards points to the player if the scene is active.
    /// </summary>
    void OnDestroy()
    {
        // Only award score during active gameplay (not when scene is unloading)
        if (gameObject.scene.isLoaded)
        {
            ScoreManager.score++;
        }
    }
}
