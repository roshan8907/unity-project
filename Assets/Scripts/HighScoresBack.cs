using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Provides functionality for returning to the main menu from the high scores screen.
/// </summary>
public class HighScoresBack : MonoBehaviour
{
    /// <summary>
    /// Loads the MainMenu scene. Attach this to a UI button's OnClick event.
    /// </summary>
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
