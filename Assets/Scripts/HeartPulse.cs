using UnityEngine;

/// <summary>
/// Attach this to the HeartPickup prefab.
/// It pulses the heart between a specific min and max scale to create a beating effect.
/// It NEVER scales relative to anything — it always uses absolute values.
/// </summary>
public class HeartPulse : MonoBehaviour
{
    // Fixed base scale — matches prefab scale of 0.25
    private const float minScale = 0.25f;
    private const float maxScale = 0.30f;

    /// <summary>
    /// Pulses the scale of the GameObject continuously using a sine wave.
    /// </summary>
    void Update()
    {
        // Sine wave oscillates between -1 and 1
        // We map it to minScale through maxScale
        float t = (Mathf.Sin(Time.time * 5f) + 1f) / 2f; // 0.0 to 1.0
        float scale = Mathf.Lerp(minScale, maxScale, t);

        // Always set ABSOLUTE scale — never multiply by existing scale
        transform.localScale = new Vector3(scale, scale, 1f);
    }
}
