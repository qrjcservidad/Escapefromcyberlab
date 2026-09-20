using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target Settings")]
    [Tooltip("Drag your Player GameObject/Transform here.")]
    public Transform player;

    [Header("Camera Offset & Position")]
    [Tooltip("Distance from the player character.")]
    public float distance = 5.0f;

    [Tooltip("Vertical height offset relative to the player position.")]
    public float heightOffset = 1.5f;

    [Tooltip("Horizontal offset (e.g., set to 0.5f for an over-the-shoulder view).")]
    public float sideOffset = 0.0f;

    [Header("Controls & Sensitivity")]
    [Tooltip("Sensitivity multiplier for mouse movement.")]
    public float mouseSensitivity = 3.0f;

    [Header("Pitch (Vertical Rotation) Limits")]
    [Tooltip("Minimum vertical angle (looking up).")]
    public float pitchMin = -10f;

    [Tooltip("Maximum vertical angle (looking down).")]
    public float pitchMax = 70f;

    private float currentYaw = 0f;
    private float currentPitch = 20f;

    void Start()
    {
        // Lock and hide mouse cursor during play
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Mouse input for rotation
        currentYaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        currentPitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Clamp pitch to prevent camera flipping upside down
        currentPitch = Mathf.Clamp(currentPitch, pitchMin, pitchMax);

        // Rotation matrix based on pitch and yaw
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);

        // Target look position (player position + height/side offsets)
        Vector3 focusPoint = player.position + Vector3.up * heightOffset + (rotation * Vector3.right * sideOffset);

        // Calculate final camera position relative to focus point
        Vector3 targetPosition = focusPoint - (rotation * Vector3.forward * distance);

        // Apply position and rotation
        transform.position = targetPosition;
        transform.LookAt(focusPoint);
    }
}