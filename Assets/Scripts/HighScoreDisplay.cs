using UnityEngine;
using TMPro;

/// <summary>
/// Handles the display of the player's highest score in the UI.
/// </summary>
public class HighScoreDisplay : MonoBehaviour
{
    /// <summary>
    /// The TextMeshPro UI element used to display the high score.
    /// </summary>
    public TextMeshProUGUI scoreText;

    /// <summary>
    /// Retrieves the saved high score from PlayerPrefs and updates the text display on start.
    /// </summary>
    void Start()
    {
        // Retrieve the saved "HighScore" value. Defaults to 0 if not found.
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        scoreText.text = "Best Score: " + highScore;
    }
}