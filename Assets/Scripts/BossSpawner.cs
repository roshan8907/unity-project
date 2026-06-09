using UnityEngine;

/// <summary>
/// Handles the spawning of the boss once all smaller enemies are defeated.
/// </summary>
public class BossSpawner : MonoBehaviour
{
    /// <summary>
    /// The Boss GameObject to activate.
    /// </summary>
    public GameObject boss;

    /// <summary>
    /// Reference to the PlayerAttack script to unlock the gun when the boss spawns.
    /// </summary>
    public PlayerAttack playerAttack;

    /// <summary>
    /// The AudioSource for the boss's introductory sound.
    /// </summary>
    public AudioSource bossIntroSound;

    /// <summary>
    /// The AudioSource for the background music during the boss fight.
    /// </summary>
    public AudioSource bossFightMusic;

    private bool bossSpawned = false;

    /// <summary>
    /// Attempts to find the PlayerAttack component on the player if not manually assigned.
    /// </summary>
    void Start()
    {
        if (playerAttack == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerAttack = player.GetComponent<PlayerAttack>();
            }
        }
    }

    /// <summary>
    /// Continuously checks the number of remaining small enemies. 
    /// If none are left, triggers the boss spawn and unlocks the player's gun.
    /// </summary>
    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int smallEnemies = 0;

        // Count remaining small enemies
        foreach (GameObject enemy in enemies)
        {
            EnemyHealth eh = enemy.GetComponent<EnemyHealth>();
            if (eh != null && !eh.isBoss)
            {
                smallEnemies++;
            }
        }

        // Spawn boss if all small enemies are defeated and it hasn't been spawned yet
        if (smallEnemies == 0 && !bossSpawned)
        {
            bossSpawned = true;
            boss.SetActive(true);

            // Double check for player attack script
            if (playerAttack == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    playerAttack = player.GetComponent<PlayerAttack>();
                }
            }

            // Unlock the player's gun
            if (playerAttack != null)
            {
                playerAttack.hasGun = true;
                Debug.Log("Gun unlocked!");
            }

            // Play the boss intro sound
            if (bossIntroSound != null)
            {
                bossIntroSound.Play();
            }

            // Start fight music after a 2-second delay to let the intro sound play
            Invoke(nameof(StartFightMusic), 2f);
        }
    }

    /// <summary>
    /// Plays the boss fight music. Called via Invoke to delay its start.
    /// </summary>
    void StartFightMusic()
    {
        if (bossFightMusic != null)
        {
            bossFightMusic.Play();
        }
    }
}