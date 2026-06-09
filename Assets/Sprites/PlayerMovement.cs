using UnityEngine;

/// <summary>
/// Controls the movement and facing direction of the player character.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// The speed at which the player moves.
    /// </summary>
    public float moveSpeed = 5f;

    /// <summary>
    /// Indicates the current facing direction: 1 for right, -1 for left.
    /// </summary>
    public float facingDirection = 1f;

    private Rigidbody2D rb;
    private Vector2 movement;

    /// <summary>
    /// Initializes the Rigidbody2D and ensures Z-axis rotation is frozen.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    /// <summary>
    /// Gathers input and updates the character's facing direction by flipping the local scale.
    /// </summary>
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x > 0f)
        {
            facingDirection = 1f;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (movement.x < 0f)
        {
            facingDirection = -1f;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    /// <summary>
    /// Applies movement to the Rigidbody2D using physics.
    /// </summary>
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}