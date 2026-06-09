using UnityEngine;

/// <summary>
/// Teleports the player to a specified target location when they enter the trigger.
/// </summary>
public class TeleportHole : MonoBehaviour
{
    /// <summary>
    /// The transform representing the destination of the teleportation.
    /// </summary>
    public Transform teleportTarget;

    private bool canTeleport = true;

    private AudioSource teleportSound;

    /// <summary>
    /// Retrieves the attached AudioSource component.
    /// </summary>
    void Start()
    {
        teleportSound = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Triggers teleportation if the player enters and teleportation is allowed.
    /// </summary>
    /// <param name="other">The collider entering the trigger.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canTeleport)
        {
            other.transform.position = teleportTarget.position;

            if (teleportSound != null)
            {
                teleportSound.Play();
            }

            canTeleport = false;

            // Prevent immediate re-teleportation back from the destination hole
            TeleportHole otherHole = teleportTarget.GetComponent<TeleportHole>();

            if (otherHole != null)
            {
                otherHole.canTeleport = false;
            }
        }
    }

    /// <summary>
    /// Re-enables teleportation once the player exits the trigger.
    /// </summary>
    /// <param name="other">The collider exiting the trigger.</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canTeleport = true;
        }
    }
}