using UnityEngine;

/// <summary>
/// Generates procedural dungeon textures and spawns decorative elements.
/// </summary>
public class DungeonGenerator : MonoBehaviour
{
    /// <summary>
    /// Determines whether decorative elements (rocks, bones, torches, etc.) should be generated.
    /// </summary>
    [Header("Generation Settings")]
    public bool generateDecorations = true;

    /// <summary>
    /// The number of random decorations to spawn on the floor.
    /// </summary>
    public int decorationCount = 30;

    /// <summary>
    /// Custom floor sprite. If null, a procedural stone tile texture will be generated.
    /// </summary>
    [Header("Decoration Prefabs (Optional)")]
    public Sprite customFloorSprite;

    /// <summary>
    /// Custom wall sprite. If null, a procedural brick texture will be generated.
    /// </summary>
    public Sprite customWallSprite;

    /// <summary>
    /// Applies textures and conditionally spawns decorations when the script starts.
    /// </summary>
    private void Start()
    {
        ApplyDungeonTextures();
        if (generateDecorations)
        {
            SpawnDungeonDecorations();
        }
    }

    /// <summary>
    /// Applies either the custom assigned sprites or procedurally generated sprites to the floor and walls.
    /// </summary>
    private void ApplyDungeonTextures()
    {
        // 1. Generate Floor Sprite
        Sprite floorSprite = customFloorSprite;
        if (floorSprite == null)
        {
            floorSprite = CreateStoneTileSprite(
                new Color(0.11f, 0.11f, 0.14f), // Base dark slate gray
                new Color(0.06f, 0.06f, 0.08f), // Dark grout/border lines
                new Color(0.16f, 0.16f, 0.20f)  // Lighter highlights
            );
        }

        // 2. Generate Wall Sprite
        Sprite wallSprite = customWallSprite;
        if (wallSprite == null)
        {
            wallSprite = CreateBrickWallSprite(
                new Color(0.08f, 0.08f, 0.10f), // Darker wall base
                new Color(0.04f, 0.04f, 0.06f), // Deep grout lines
                new Color(0.12f, 0.12f, 0.15f)  // Highlights
            );
        }

        // 3. Find Floor and apply
        GameObject floorObj = GameObject.Find("Floor");
        if (floorObj != null)
        {
            SpriteRenderer sr = floorObj.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = floorSprite;
                sr.color = Color.white; // Reset coloring to let texture shine
                sr.drawMode = SpriteDrawMode.Tiled;
                sr.tileMode = SpriteTileMode.Continuous;
            }
        }

        // 4. Find Walls and apply
        string[] wallNames = { "WallLeft", "WallRight", "WallTop", "WallBottom" };
        foreach (string wallName in wallNames)
        {
            GameObject wallObj = GameObject.Find(wallName);
            if (wallObj != null)
            {
                SpriteRenderer sr = wallObj.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.sprite = wallSprite;
                    sr.color = Color.white;
                    sr.drawMode = SpriteDrawMode.Tiled;
                    sr.tileMode = SpriteTileMode.Continuous;
                }
            }
        }
    }

    /// <summary>
    /// Spawns randomly positioned procedural decorations (rocks, cracks, bones) on the floor,
    /// and places torches along the top wall.
    /// </summary>
    private void SpawnDungeonDecorations()
    {
        // Find play area boundaries using Floor
        GameObject floorObj = GameObject.Find("Floor");
        if (floorObj == null)
        {
            return;
        }

        Vector2 floorScale = floorObj.transform.localScale;
        float minX = -floorScale.x / 2.3f;
        float maxX = floorScale.x / 2.3f;
        float minY = -floorScale.y / 2.3f;
        float maxY = floorScale.y / 2.3f;

        // Generate decoration sprites
        Sprite rockSprite = CreateDecorationSprite(0);
        Sprite crackSprite = CreateDecorationSprite(1);
        Sprite boneSprite = CreateDecorationSprite(2);
        Sprite torchSprite = CreateDecorationSprite(3);

        // Spawn items randomly
        for (int i = 0; i < decorationCount; i++)
        {
            Vector2 spawnPos = new Vector2(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY)
            );

            // Avoid spawning directly at origin where Player starts
            if (Vector2.Distance(spawnPos, Vector2.zero) < 2f)
            {
                continue;
            }

            int decType = Random.Range(0, 3); // 0 = Rock, 1 = Crack, 2 = Bone
            GameObject dec = new GameObject("DungeonDecoration_" + i);
            dec.transform.position = spawnPos;
            dec.transform.parent = transform;

            SpriteRenderer sr = dec.AddComponent<SpriteRenderer>();
            sr.sortingOrder = -1; // Render behind player and enemies but in front of floor

            if (decType == 0)
            {
                sr.sprite = rockSprite;
                dec.transform.localScale = Vector3.one * Random.Range(0.8f, 1.4f);
            }
            else if (decType == 1)
            {
                sr.sprite = crackSprite;
            }
            else
            {
                sr.sprite = boneSprite;
                dec.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            }
        }

        // Spawn Torches along the top wall
        GameObject topWall = GameObject.Find("WallTop");
        if (topWall != null)
        {
            Vector3 topWallPos = topWall.transform.position;
            float wallWidth = topWall.transform.localScale.x;

            int torchCount = Mathf.Clamp((int)(wallWidth / 8f), 2, 6);
            for (int i = 0; i < torchCount; i++)
            {
                float t = (float)(i + 1) / (torchCount + 1);
                float x = Mathf.Lerp(-wallWidth / 2f + 1f, wallWidth / 2f - 1f, t);

                GameObject torch = new GameObject("WallTorch_" + i);
                torch.transform.position = new Vector3(x, topWallPos.y - 0.7f, 0f);
                torch.transform.parent = transform;

                SpriteRenderer sr = torch.AddComponent<SpriteRenderer>();
                sr.sprite = torchSprite;
                sr.sortingOrder = 2; // Render in front of walls

                // Add torch light flicker script
                torch.AddComponent<TorchFlicker>();
            }
        }
    }

    // --- Procedural Sprite Generation Functions ---

    /// <summary>
    /// Procedurally generates a stone tile sprite using the specified colors.
    /// </summary>
    /// <param name="baseColor">The main color of the stone.</param>
    /// <param name="borderColor">The color for the stone's borders (grout).</param>
    /// <param name="highlightColor">The color used for highlighting edges.</param>
    /// <returns>A procedural stone tile Sprite.</returns>
    private Sprite CreateStoneTileSprite(Color baseColor, Color borderColor, Color highlightColor)
    {
        int size = 16;
        Texture2D texture = new Texture2D(size, size);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Repeat;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Color pixelColor = baseColor;

                // Draw dark borders to represent stone tiles
                if (x == 0 || y == 0 || x == size - 1 || y == size - 1)
                {
                    pixelColor = borderColor;
                }
                // Lighter highlights on the inner top/left borders for 3D depth
                else if (x == 1 || y == 1)
                {
                    pixelColor = highlightColor;
                }
                // Add minor noise texture for a stone-like look
                else
                {
                    float noise = Random.Range(-0.02f, 0.02f);
                    pixelColor = new Color(
                        Mathf.Clamp01(baseColor.r + noise),
                        Mathf.Clamp01(baseColor.g + noise),
                        Mathf.Clamp01(baseColor.b + noise)
                    );
                }

                texture.SetPixel(x, y, pixelColor);
            }
        }

        texture.Apply();
        return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 16f);
    }

    /// <summary>
    /// Procedurally generates a brick wall sprite using an offset running bond pattern.
    /// </summary>
    /// <param name="baseColor">The main color of the bricks.</param>
    /// <param name="borderColor">The color for the grout lines between bricks.</param>
    /// <param name="highlightColor">The color used to highlight the top edges of bricks.</param>
    /// <returns>A procedural brick wall Sprite.</returns>
    private Sprite CreateBrickWallSprite(Color baseColor, Color borderColor, Color highlightColor)
    {
        int size = 16;
        Texture2D texture = new Texture2D(size, size);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Repeat;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Color pixelColor = baseColor;

                // Create brick pattern lines
                bool isBorderY = (y == 0 || y == 8 || y == size - 1);
                bool isBorderX = false;

                // Offset the vertical grout lines for classic running bond brick pattern
                if (y > 0 && y < 8)
                {
                    isBorderX = (x == 0 || x == size - 1);
                }
                else if (y > 8 && y < size - 1)
                {
                    isBorderX = (x == 8);
                }

                if (isBorderY || isBorderX)
                {
                    pixelColor = borderColor;
                }
                else if (y == 1 || y == 9 || (isBorderX && x == 1))
                {
                    pixelColor = highlightColor;
                }
                else
                {
                    // Slate stone brick texture variation
                    float noise = Random.Range(-0.03f, 0.03f);
                    pixelColor = new Color(
                        Mathf.Clamp01(baseColor.r + noise),
                        Mathf.Clamp01(baseColor.g + noise),
                        Mathf.Clamp01(baseColor.b + noise)
                    );
                }

                texture.SetPixel(x, y, pixelColor);
            }
        }

        texture.Apply();
        return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 16f);
    }

    /// <summary>
    /// Procedurally generates small decoration sprites based on the given type.
    /// </summary>
    /// <param name="type">The type of decoration: 0 for Rock, 1 for Crack, 2 for Bone, 3 for Torch.</param>
    /// <returns>A procedural decoration Sprite.</returns>
    private Sprite CreateDecorationSprite(int type)
    {
        int size = 16;
        Texture2D texture = new Texture2D(size, size);
        texture.filterMode = FilterMode.Point;

        // Clear texture (transparent)
        Color transparent = new Color(0, 0, 0, 0);
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                texture.SetPixel(x, y, transparent);
            }
        }

        if (type == 0) // Rock
        {
            Color stoneColor = new Color(0.25f, 0.25f, 0.28f);
            Color stoneDark = new Color(0.15f, 0.15f, 0.18f);

            // Draw a small rocky blob
            DrawBlob(texture, 8, 8, 4, stoneColor, stoneDark);
        }
        else if (type == 1) // Crack
        {
            Color crackColor = new Color(0.04f, 0.04f, 0.06f);
            // Draw a jagged line
            texture.SetPixel(6, 6, crackColor);
            texture.SetPixel(7, 7, crackColor);
            texture.SetPixel(8, 7, crackColor);
            texture.SetPixel(9, 8, crackColor);
            texture.SetPixel(10, 9, crackColor);
            texture.SetPixel(9, 6, crackColor);
        }
        else if (type == 2) // Bone
        {
            Color boneColor = new Color(0.85f, 0.85f, 0.8f);
            // Draw a simple bone shape
            texture.SetPixel(5, 8, boneColor);
            texture.SetPixel(6, 8, boneColor);
            texture.SetPixel(7, 8, boneColor);
            texture.SetPixel(8, 8, boneColor);
            texture.SetPixel(9, 8, boneColor);
            // Bone caps
            texture.SetPixel(4, 7, boneColor);
            texture.SetPixel(4, 9, boneColor);
            texture.SetPixel(10, 7, boneColor);
            texture.SetPixel(10, 9, boneColor);
        }
        else if (type == 3) // Torch
        {
            Color woodColor = new Color(0.45f, 0.25f, 0.15f);
            Color fireColor = new Color(1.0f, 0.55f, 0.05f);

            // Wood stick
            texture.SetPixel(8, 4, woodColor);
            texture.SetPixel(8, 5, woodColor);
            texture.SetPixel(8, 6, woodColor);
            texture.SetPixel(9, 6, woodColor);

            // Torch metal holder
            texture.SetPixel(7, 7, new Color(0.3f, 0.3f, 0.35f));
            texture.SetPixel(8, 7, new Color(0.3f, 0.3f, 0.35f));
            texture.SetPixel(9, 7, new Color(0.3f, 0.3f, 0.35f));

            // Fire flame
            texture.SetPixel(8, 8, fireColor);
            texture.SetPixel(7, 9, fireColor);
            texture.SetPixel(8, 9, Color.yellow);
            texture.SetPixel(9, 9, fireColor);
            texture.SetPixel(8, 10, fireColor);
        }

        texture.Apply();
        return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 16f);
    }

    /// <summary>
    /// Helper function to draw a filled circle (blob) on a Texture2D with an outline.
    /// </summary>
    /// <param name="tex">The target Texture2D.</param>
    /// <param name="cx">Center X position.</param>
    /// <param name="cy">Center Y position.</param>
    /// <param name="radius">The radius of the blob.</param>
    /// <param name="color">The fill color.</param>
    /// <param name="outlineColor">The outline color.</param>
    private void DrawBlob(Texture2D tex, int cx, int cy, int radius, Color color, Color outlineColor)
    {
        for (int y = cy - radius; y <= cy + radius; y++)
        {
            for (int x = cx - radius; x <= cx + radius; x++)
            {
                if (x >= 0 && x < tex.width && y >= 0 && y < tex.height)
                {
                    float dist = Vector2.Distance(new Vector2(x, y), new Vector2(cx, cy));
                    if (dist < radius - 0.5f)
                    {
                        tex.SetPixel(x, y, color);
                    }
                    else if (dist < radius + 0.5f)
                    {
                        tex.SetPixel(x, y, outlineColor);
                    }
                }
            }
        }
    }
}

/// <summary>
/// Simple organic flicker script for Torches to simulate a dynamic flame.
/// </summary>
public class TorchFlicker : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color originalColor;
    private Vector3 originalScale;

    /// <summary>
    /// Initializes the base color and scale to be used for the flickering effect.
    /// </summary>
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        originalScale = transform.localScale;
    }

    /// <summary>
    /// Modulates color and scale continuously to represent a glowing, flickering flame.
    /// </summary>
    private void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time * 7.5f, transform.position.x * 12.3f);
        sr.color = Color.Lerp(originalColor, new Color(1f, 0.88f, 0.45f, 1f), noise * 0.4f);
        transform.localScale = originalScale * (1f + (noise - 0.5f) * 0.2f);
    }
}
