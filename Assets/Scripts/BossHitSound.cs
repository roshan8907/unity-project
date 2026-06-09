using UnityEngine;

/// <summary>
/// Plays a heavy monster hit sound when the boss deals damage to the player.
/// Attach this ONLY to the Boss GameObject (alongside EnemyDamage).
/// This script does NOT modify EnemyDamage logic at all.
/// It independently detects boss-to-player collision and plays a sound.
/// Normal enemies are unaffected — they don't have this script.
/// </summary>
public class BossHitSound : MonoBehaviour
{
    /// <summary>
    /// AudioClip for the heavy boss hit. Assign in Inspector, or leave null to use a procedural thud.
    /// </summary>
    [Header("Boss Hit Sound")]
    [Tooltip("AudioClip for the heavy boss hit. Assign in Inspector, or leave null to use a procedural thud.")]
    public AudioClip bossHitClip;

    /// <summary>
    /// The volume of the hit sound.
    /// </summary>
    [Range(0.5f, 1.5f)]
    public float volume = 1.0f;

    /// <summary>
    /// The pitch of the hit sound. Lower pitch equals a deeper, scarier sound.
    /// </summary>
    [Range(0.3f, 1.0f)]
    [Tooltip("Lower pitch = deeper, scarier sound")]
    public float pitch = 0.6f;

    /// <summary>
    /// Minimum seconds between hit sounds to avoid spamming multiple sounds at once.
    /// </summary>
    [Tooltip("Minimum seconds between hit sounds to avoid spamming")]
    public float soundCooldown = 0.8f;

    private AudioSource audioSource;
    private float lastSoundTime;

    /// <summary>
    /// Initializes the AudioSource and generates a procedural sound if no clip is assigned.
    /// </summary>
    void Start()
    {
        // Create a dedicated AudioSource for boss hit sounds
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 2D sound
        audioSource.volume = volume;
        audioSource.pitch = pitch;

        // If no clip assigned, generate a deep procedural thud
        if (bossHitClip == null)
        {
            bossHitClip = GenerateDeepThud();
        }

        audioSource.clip = bossHitClip;
        lastSoundTime = -soundCooldown;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryPlayHitSound(collision.gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryPlayHitSound(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryPlayHitSound(other.gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryPlayHitSound(other.gameObject);
    }

    /// <summary>
    /// Checks if the collided object is the player and plays the hit sound if the cooldown has elapsed.
    /// </summary>
    /// <param name="target">The GameObject that the boss collided with.</param>
    private void TryPlayHitSound(GameObject target)
    {
        // Only play for player hits
        if (!target.CompareTag("Player") && target.GetComponentInParent<PlayerHealth>() == null)
        {
            return;
        }

        // Respect cooldown to prevent overlapping sounds
        if (Time.time < lastSoundTime + soundCooldown)
        {
            return;
        }

        lastSoundTime = Time.time;

        if (audioSource != null && bossHitClip != null)
        {
            // Add slight pitch variation for a more natural effect
            audioSource.pitch = pitch + Random.Range(-0.08f, 0.08f);
            audioSource.PlayOneShot(bossHitClip, volume);
        }
    }

    /// <summary>
    /// Generates a deep, heavy impact/thud sound procedurally.
    /// Combines a low-frequency slam with noise burst for that "monster hit" feel.
    /// </summary>
    /// <returns>A procedurally generated AudioClip representing a thud.</returns>
    private AudioClip GenerateDeepThud()
    {
        int sampleRate = 44100;
        float duration = 0.6f;
        int sampleCount = (int)(sampleRate * duration);
        float[] samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = (float)i / sampleRate;
            float normalizedTime = (float)i / sampleCount;

            // Envelope: sharp attack, heavy decay
            float envelope;
            if (normalizedTime < 0.02f)
            {
                // Fast attack
                envelope = normalizedTime / 0.02f;
            }
            else
            {
                // Exponential decay
                envelope = Mathf.Exp(-(normalizedTime - 0.02f) * 6f);
            }

            // Layer 1: Deep bass thud (50-60Hz)
            float bass = Mathf.Sin(2f * Mathf.PI * 55f * t) * 0.7f;

            // Layer 2: Sub-bass rumble (30Hz)
            float subBass = Mathf.Sin(2f * Mathf.PI * 30f * t) * 0.4f;

            // Layer 3: Impact crack / noise burst (only in first 80ms)
            float noiseBurst = 0f;
            if (normalizedTime < 0.12f)
            {
                noiseBurst = (Random.Range(-1f, 1f)) * 0.5f * Mathf.Exp(-normalizedTime * 25f);
            }

            // Layer 4: Mid growl (120Hz with slight detuning)
            float growl = Mathf.Sin(2f * Mathf.PI * 120f * t + Mathf.Sin(t * 8f) * 0.5f) * 0.25f;

            // Combine all layers
            samples[i] = (bass + subBass + noiseBurst + growl) * envelope;

            // Soft clip to avoid distortion
            samples[i] = Mathf.Clamp(samples[i], -0.95f, 0.95f);
        }

        AudioClip clip = AudioClip.Create("BossHitThud", sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }
}
