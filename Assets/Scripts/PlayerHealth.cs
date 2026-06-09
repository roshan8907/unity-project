using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Manages the player's health, handles damage and healing, and triggers game over logic.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    /// <summary>
    /// The maximum possible health the player can have.
    /// </summary>
    [Header("Health Settings")]
    public float maxHealth = 100f;

    /// <summary>
    /// The current health of the player.
    /// </summary>
    public float health = 100f;

    /// <summary>
    /// The UI text element that displays the player's current health.
    /// </summary>
    [Header("UI")]
    public TextMeshProUGUI healthText;

    /// <summary>
    /// The sound played when the player takes damage.
    /// </summary>
    [Header("Sounds")]
    public AudioSource hurtSound;

    /// <summary>
    /// The sound played when the player's health reaches zero.
    /// </summary>
    public AudioSource gameOverSound;

    private bool isDead = false;

    /// <summary>
    /// Initializes health and updates the UI on start.
    /// </summary>
    void Start()
    {
        health = maxHealth;
        UpdateUI();
    }

    /// <summary>
    /// Checks if health has reached zero and handles the death sequence.
    /// </summary>
    void Update()
    {
        UpdateUI();

        if (health <= 0 && !isDead)
        {
            isDead = true;

            if (gameOverSound != null)
            {
                gameOverSound.Play();
            }

            // wait for full sound before scene change
            Invoke("LoadGameOver", 3f);
        }
    }

    /// <summary>
    /// Reduces the player's health by the specified amount and plays the hurt sound.
    /// </summary>
    /// <param name="damage">The amount of damage to take.</param>
    public void TakeDamage(float damage)
    {
        if (isDead)
        {
            return;
        }

        health -= damage;

        health = Mathf.Clamp(health, 0f, maxHealth);

        if (hurtSound != null)
        {
            hurtSound.Play();
        }

        UpdateUI();
    }

    /// <summary>
    /// Increases the player's health by the specified amount, up to the maximum.
    /// </summary>
    /// <param name="amount">The amount of health to restore.</param>
    public void Heal(float amount)
    {
        health += amount;

        health = Mathf.Clamp(health, 0f, maxHealth);

        UpdateUI();
    }

    /// <summary>
    /// Transitions to the Game Over scene.
    /// </summary>
    void LoadGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    /// <summary>
    /// Updates the health UI text.
    /// </summary>
    void UpdateUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + health;
        }
    }
}