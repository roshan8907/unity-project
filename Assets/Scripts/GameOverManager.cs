using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages scene transitions and game exits specifically for game over or menu contexts.
/// </summary>
public class GameOverManager : MonoBehaviour
{
    /// <summary>
    /// Reloads the main gameplay scene to restart the game.
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    /// <summary>
    /// Loads the main menu scene.
    /// </summary>
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Quits the game application completely.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();

        Debug.Log("Game Closed");
    }
}