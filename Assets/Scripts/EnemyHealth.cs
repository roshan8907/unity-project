using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the health, damage reception, and death logic for enemies and bosses.
/// </summary>
public class EnemyHealth : MonoBehaviour
{
    /// <summary>
    /// The starting health points of the enemy.
    /// </summary>
    public int health = 100;

    /// <summary>
    /// Indicates whether this enemy is a boss character.
    /// </summary>
    public bool isBoss = false;

    /// <summary>
    /// Reference to the UI text displaying the boss's health.
    /// </summary>
    public TextMeshProUGUI bossHealthText;

    /// <summary>
    /// Reduces the enemy's health by the specified amount and checks for death.
    /// </summary>
    /// <param name="damage">The amount of damage to subtract from health.</param>
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (isBoss && bossHealthText != null)
        {
            bossHealthText.text = "Boss HP: " + health;
        }

        if (health <= 0)
        {
            // Award score for killing this enemy
            ScoreManager.score++;

            if (isBoss)
            {
                SceneManager.LoadScene("WinScene");
            }

            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Initializes the boss health UI if applicable.
    /// </summary>
    void Start()
    {
        if (isBoss && bossHealthText != null)
        {
            bossHealthText.text = "Boss HP: " + health;
        }
    }
}