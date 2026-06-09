using UnityEngine;

/// <summary>
/// Handles pausing and unpausing the game through user input.
/// </summary>
public class PauseManager : MonoBehaviour
{
    private bool paused = false;

    /// <summary>
    /// Checks for the 'P' key input to toggle the game's paused state by adjusting Time.timeScale.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            paused = !paused;

            if (paused)
            {
                // Freeze game time
                Time.timeScale = 0f;
            }
            else
            {
                // Resume game time
                Time.timeScale = 1f;
            }
        }
    }
}