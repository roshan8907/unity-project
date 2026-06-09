using UnityEngine;

/// <summary>
/// Provides functionality to close the application.
/// </summary>
public class ExitGame : MonoBehaviour
{
    /// <summary>
    /// Quits the game application. This will close the built executable but will only log in the Unity Editor.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("Game Closed");
    }
}