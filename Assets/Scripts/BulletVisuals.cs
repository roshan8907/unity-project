using UnityEngine;

/// <summary>
/// Purely visual enhancement for bullets.
/// Attach this to the Bullet prefab alongside the existing Bullet script.
/// Creates a glowing fireball look using procedural sprite and trail.
/// Does NOT touch any gameplay logic (damage, speed, collision).
/// </summary>
public class BulletVisuals : MonoBehaviour
{
    /// <summary>
    /// The core color of the bullet glow (center).
    /// </summary>
    [Header("Glow Settings")]
    public Color coreColor = new Color(1f, 0.95f, 0.3f, 1f);   // bright yellow center

    /// <summary>
    /// The mid-layer color of the bullet glow.
    /// </summary>
    public Color midColor  = new Color(1f, 0.5f, 0.05f, 0.9f);  // orange mid

    /// <summary>
    /// The edge color of the bullet glow (fades to transparent).
    /// </summary>
    public Color edgeColor = new Color(1f, 0.15f, 0.0f, 0.0f);  // red edge, transparent

    /// <summary>
    /// The base scale of the bullet visual.
    /// </summary>
    [Header("Size")]
    public float bulletScale = 0.35f;

    /// <summary>
    /// Determines whether the bullet should have a trailing visual effect.
    /// </summary>
    [Header("Trail")]
    public bool enableTrail = true;

    /// <summary>
    /// The duration (in seconds) that the trail persists behind the bullet.
    /// </summary>
    public float trailTime = 0.15f;

    /// <summary>
    /// Initializes the procedural sprite, optional trail, and visuals.
    /// </summary>
    void Start()
    {
        // --- 1. Create a procedural glowing circle sprite ---
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            sr = gameObject.AddComponent<SpriteRenderer>();
        }

        // Generate a soft radial gradient texture
        int texSize = 64;
        Texture2D tex = new Texture2D(texSize, texSize, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Bilinear;
        tex.wrapMode = TextureWrapMode.Clamp;

        Vector2 center = new Vector2(texSize / 2f, texSize / 2f);
        float maxRadius = texSize / 2f;

        for (int y = 0; y < texSize; y++)
        {
            for (int x = 0; x < texSize; x++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), center);
                float t = Mathf.Clamp01(dist / maxRadius);

                Color pixel;
                if (t < 0.3f)
                {
                    // Core: bright yellow
                    pixel = Color.Lerp(coreColor, midColor, t / 0.3f);
                }
                else if (t < 0.7f)
                {
                    // Mid: orange to red
                    pixel = Color.Lerp(midColor, edgeColor, (t - 0.3f) / 0.4f);
                }
                else
                {
                    // Edge: fade out
                    pixel = edgeColor;
                    pixel.a *= Mathf.Clamp01((1f - t) / 0.3f);
                }

                tex.SetPixel(x, y, pixel);
            }
        }
        tex.Apply();

        Sprite glowSprite = Sprite.Create(
            tex,
            new Rect(0, 0, texSize, texSize),
            new Vector2(0.5f, 0.5f),
            texSize
        );

        sr.sprite = glowSprite;
        sr.color = Color.white; // tint handled in texture
        sr.sortingOrder = 10;   // render above most things

        // Scale the bullet up to be clearly visible
        transform.localScale = new Vector3(bulletScale, bulletScale, 1f);

        // --- 2. Optional short trail ---
        if (enableTrail)
        {
            TrailRenderer trail = gameObject.AddComponent<TrailRenderer>();
            trail.time = trailTime;
            trail.startWidth = bulletScale * 0.5f;
            trail.endWidth = 0f;
            trail.material = new Material(sr.material);
            trail.sortingOrder = 9;

            // Gradient: orange to transparent
            Gradient grad = new Gradient();
            grad.SetKeys(
                new GradientColorKey[]
                {
                    new GradientColorKey(new Color(1f, 0.6f, 0.1f), 0f),
                    new GradientColorKey(new Color(1f, 0.2f, 0.0f), 1f)
                },
                new GradientAlphaKey[]
                {
                    new GradientAlphaKey(0.7f, 0f),
                    new GradientAlphaKey(0f, 1f)
                }
            );
            trail.colorGradient = grad;
            trail.minVertexDistance = 0.05f;
        }
    }

    /// <summary>
    /// Updates the bullet's scale dynamically to create a pulsing "energy" effect.
    /// </summary>
    void Update()
    {
        // Subtle size pulse for "energy" feel
        float pulse = 1f + Mathf.Sin(Time.time * 12f) * 0.08f;
        float s = bulletScale * pulse;
        transform.localScale = new Vector3(s, s, 1f);
    }
}
