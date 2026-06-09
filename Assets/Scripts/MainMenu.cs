using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the functionality for the main menu buttons.
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Loads the first gameplay level.
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    /// <summary>
    /// Loads the High Scores display scene.
    /// </summary>
    public void HighScores()
    {
        SceneManager.LoadScene("HighScores");
    }

    /// <summary>
    /// Quits the application.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}