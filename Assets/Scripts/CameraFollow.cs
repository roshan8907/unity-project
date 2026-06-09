using UnityEngine;

/// <summary>
/// Smoothly follows a target (usually the player) with the camera.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    /// <summary>
    /// The target Transform that the camera should follow.
    /// </summary>
    [Header("Follow Settings")]
    public Transform target;

    /// <summary>
    /// Interpolation speed for camera movement (higher values mean faster movement).
    /// </summary>
    public float smoothSpeed = 0.125f;

    /// <summary>
    /// The default 2D offset from the target, primarily used to keep the camera along the correct Z plane.
    /// </summary>
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    /// <summary>
    /// Attempts to auto-locate the player target if it hasn't been manually assigned.
    /// </summary>
    void Start()
    {
        // Auto-locate target if not manually assigned
        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                target = playerObj.transform;
            }
        }
    }

    /// <summary>
    /// Smoothly interpolates the camera's position towards the target's position each frame.
    /// LateUpdate is used to ensure the target has already moved before the camera updates.
    /// </summary>
    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        // Calculate target position with offset
        Vector3 desiredPosition = target.position + offset;

        // Smoothly interpolate between camera's current position and target position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Assign to transform
        transform.position = smoothedPosition;
    }
}
