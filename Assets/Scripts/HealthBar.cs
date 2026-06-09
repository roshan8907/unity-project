using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the player health bar UI in Unity 6.
/// Automatically handles references, checks for errors, and updates the fill amount.
/// </summary>
public class HealthBar : MonoBehaviour
{
    /// <summary>
    /// The Image component used to visually represent the health bar's fill.
    /// </summary>
    [Header("UI Components")]
    [Tooltip("Drag the HealthBarFill Image component here from the Hierarchy")]
    public Image fillImage;

    /// <summary>
    /// Reference to the PlayerHealth script to read the current and max health.
    /// </summary>
    [Header("References")]
    [Tooltip("Drag the Player GameObject or PlayerHealth script here. If left blank, it will auto-locate via the 'Player' tag.")]
    public PlayerHealth playerHealth;

    // Safety and optimization flags to prevent console spam
    private bool hasLoggedMissingFill = false;
    private bool hasLoggedMissingPlayer = false;
    private float lastLoggedFillAmount = -1f;

    /// <summary>
    /// Initializes the UI components and attempts to find references if they are missing.
    /// </summary>
    void Start()
    {
        Debug.Log($"[HealthBar] Start: Initialized on {gameObject.name}. " +
                  $"Fill Image: {(fillImage != null ? "Assigned" : "MISSING")}, " +
                  $"PlayerHealth: {(playerHealth != null ? "Assigned" : "MISSING")}");

        // 1. Auto-Configure Fill Image settings to make it bulletproof
        if (fillImage != null)
        {
            ConfigureFillImageSettings();
        }

        // 2. Auto-locate PlayerHealth if it wasn't dragged into the Inspector
        if (playerHealth == null)
        {
            FindPlayerHealthReference();
        }
    }

    /// <summary>
    /// Updates the health bar's fill amount to match the player's current health.
    /// </summary>
    void Update()
    {
        // 1. Safety Check: Verify if Fill Image is assigned
        if (fillImage == null)
        {
            if (!hasLoggedMissingFill)
            {
                Debug.LogError($"[HealthBar] fillImage is null on {gameObject.name}! Please drag the 'HealthBarFill' Image component into the slot on this script.", this);
                hasLoggedMissingFill = true;
            }
            return;
        }

        // 2. Safety Check: Verify if PlayerHealth is assigned or auto-locate it
        if (playerHealth == null)
        {
            FindPlayerHealthReference();
            if (playerHealth == null)
            {
                if (!hasLoggedMissingPlayer)
                {
                    Debug.LogWarning($"[HealthBar] playerHealth reference is null on {gameObject.name} and could not be auto-located! " +
                                     "Please make sure your Player GameObject in the scene has the 'PlayerHealth' script attached and is tagged 'Player'.", this);
                    hasLoggedMissingPlayer = true;
                }
                return;
            }
        }

        // Reset player missing warning flag once successfully found/re-established
        hasLoggedMissingPlayer = false;

        // 3. Calculate and apply Fill Amount
        float maxHealth = playerHealth.maxHealth > 0f ? playerHealth.maxHealth : 100f;
        float targetFill = Mathf.Clamp01(playerHealth.health / maxHealth);
        
        // This makes the UI fill image shrink/grow matching the player health ratio
        fillImage.fillAmount = targetFill;

        // Log fill changes to the console to verify execution (only logs when health actually changes!)
        if (Mathf.Abs(targetFill - lastLoggedFillAmount) > 0.001f)
        {
            Debug.Log($"[HealthBar] UI Updated: Fill Amount is now {targetFill * 100f:F1}% (Health: {playerHealth.health}/{maxHealth})", this);
            lastLoggedFillAmount = targetFill;
        }
    }

    /// <summary>
    /// Automatically configures the target Image component properties to ensure
    /// it functions correctly as a horizontal filled health bar.
    /// </summary>
    private void ConfigureFillImageSettings()
    {
        bool changed = false;

        // Enforce Image Type as Filled
        if (fillImage.type != Image.Type.Filled)
        {
            fillImage.type = Image.Type.Filled;
            changed = true;
        }

        // Enforce Fill Method as Horizontal
        if (fillImage.fillMethod != Image.FillMethod.Horizontal)
        {
            fillImage.fillMethod = Image.FillMethod.Horizontal;
            changed = true;
        }

        // Enforce Fill Origin as Left (starts empty on left, fills to the right)
        if (fillImage.fillOrigin != (int)Image.OriginHorizontal.Left)
        {
            fillImage.fillOrigin = (int)Image.OriginHorizontal.Left;
            changed = true;
        }

        if (changed)
        {
            Debug.Log($"[HealthBar] Senior Developer Pro-Tip: Automatically configured '{fillImage.gameObject.name}' Image component settings " +
                      "to Type: Filled, Method: Horizontal, Origin: Left. No manual configuration needed!", this);
        }
    }

    /// <summary>
    /// Searches the scene for a GameObject with the tag 'Player' and grabs its PlayerHealth component.
    /// </summary>
    private void FindPlayerHealthReference()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                Debug.Log($"[HealthBar] Success! Auto-located PlayerHealth component on '{player.name}' and connected it.", this);
            }
            else
            {
                Debug.LogError($"[HealthBar] Found GameObject with tag 'Player' ({player.name}), but it lacks a 'PlayerHealth' script component!", this);
            }
        }
    }
}