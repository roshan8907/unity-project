using UnityEngine;

/// <summary>
/// Attach to any GameObject in Level1 to reset the score at the start of a new game.
/// This prevents score from carrying over when restarting.
/// </summary>
public class ScoreReset : MonoBehaviour
{
    /// <summary>
    /// Resets the global score to zero immediately when the scene loads.
    /// </summary>
    void Awake()
    {
        ScoreManager.score = 0;
    }
}
