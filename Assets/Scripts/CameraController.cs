using UnityEngine;
using UnityEngine.EventSystems; // KAILANGAN PARA SA MOBILE UI CHECK

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
        // Un-comment lang kung PC build, pero sa Mobile huwag i-lock para gumana ang touch
        #if !UNITY_ANDROID && !UNITY_IOS
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        #endif
    }

    void LateUpdate()
    {
        if (player == null) return;

        // I-CHECK KUNG SA UI/JOYSTICK NAKAPATONG ANG TOUCH SA MOBILE
        if (IsTouchingUI())
        {
            // HUWAG IGALAW ANG CAMERA KAPAG HAWAK ANG JOYSTICK O BUTTONS
            UpdateCameraPosition();
            return;
        }

        // Mouse/Touch input para sa camera rotation
        currentYaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        currentPitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Clamp pitch para hindi mag-flip ang camera
        currentPitch = Mathf.Clamp(currentPitch, pitchMin, pitchMax);

        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
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

    // UTILITY FUNCTION PARA I-FILTER ANG TOUCH SA JOYSTICK/BUTTONS
    private bool IsTouchingUI()
    {
        // Check para sa Mobile Touch Input
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    return true;
                }
            }
        }

        // Check para sa Mouse/Editor testing
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }

        return false;
    }
}