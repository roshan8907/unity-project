using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles button interactions for the Win Screen.
/// </summary>
public class WinMenu : MonoBehaviour
{
    /// <summary>
    /// Reloads the first level to restart the game.
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    /// <summary>
    /// Quits the application.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();

        Debug.Log("Game Closed");
    }
}