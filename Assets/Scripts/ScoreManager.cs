using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Keeps track of the current game score and updates the high score if necessary.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    /// <summary>
    /// The global score counter.
    /// </summary>
    public static int score = 0;

    /// <summary>
    /// The TextMeshPro UI element used to display the current score.
    /// </summary>
    public TextMeshProUGUI scoreText;

    /// <summary>
    /// Continuously updates the score display and checks against the saved high score.
    /// </summary>
    void Update()
    {
        scoreText.text = "Score: " + score;

        // Save High Score
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
        }
    }
}