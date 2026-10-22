using UnityEngine;
using UnityEngine.InputSystem; // Kailangan para sa Touch / On-Screen Button / Gamepad

public class PlayerMovement : MonoBehaviour
{
    private Animator anim;
    private CharacterController controller;
    private Transform cameraTransform;

    [Header("Movement Speeds")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float crouchSpeed = 1.2f; // Bilis habang naka-crouch
    public float gravity = -20f;

    [Header("Crouch Settings")]
    public float standingHeight = 2.0f; // Taas ng Character Controller kapag nakatayo
    public float crouchingHeight = 1.0f; // Taas ng Character Controller kapag naka-crouch
    public Vector3 standingCenter = new Vector3(0, 1f, 0);
    public Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);

    private Vector3 velocity;
    private bool isCrouching = false;
    private bool isCrouchUIPressed = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -5f;
        }

        float horizontal = 0f;
        float vertical = 0f;
        bool isSprintingUI = false;

        // 1. Basahin ang On-Screen Stick at Sprint/Crouch Buttons (Mobile Touch / Gamepad Controls)
        if (Gamepad.current != null)
        {
            Vector2 stickInput = Gamepad.current.leftStick.ReadValue();
            horizontal = stickInput.x;
            vertical = stickInput.y;

            isSprintingUI = Gamepad.current.buttonSouth.isPressed;
            
            // On-Screen Crouch Button Toggle (Button West / X button o custom UI binding)
            if (Gamepad.current.buttonWest.wasPressedThisFrame)
            {
                ToggleCrouch();
            }
        }

        // 2. Fallback sa Keyboard (kapag walang touch joystick input)
        if (Mathf.Approximately(horizontal, 0f) && Mathf.Approximately(vertical, 0f))
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }

        // Keyboard Crouch Trigger (C o Left Ctrl)
        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.LeftControl))
        {
            ToggleCrouch();
        }

        // Camera direction vectors
        Vector3 camForward = Vector3.forward;
        Vector3 camRight = Vector3.right;

        if (cameraTransform != null)
        {
            camForward = cameraTransform.forward;
            camRight = cameraTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();
        }

        Vector3 move = (camForward * vertical + camRight * horizontal);
        if (move.magnitude > 1f)
        {
            move.Normalize();
        }

        // Running detection (Hindi pwedeng tumakbo habang naka-crouch)
        bool isRunning = (Input.GetKey(KeyCode.LeftShift) || isSprintingUI) && !isCrouching;

        // Movement Speed logic
        float currentSpeed = walkSpeed;
        if (isCrouching)
        {
            currentSpeed = crouchSpeed;
        }
        else if (isRunning)
        {
            currentSpeed = runSpeed;
        }

        // Actual movement & rotation
        if (move.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            controller.Move(move * currentSpeed * Time.deltaTime);
        }

        // Gravity logic
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Animator speed parameter (0 = Idle, 1.0 = Walk, 2.0 = Run)
        float animSpeedParam = 0f;
        if (move.magnitude > 0.1f)
        {
            animSpeedParam = isRunning ? 2.0f : 1.0f;
        }

        // Pass parameters to Animator
        if (anim != null)
        {
            anim.SetFloat("speed", animSpeedParam);
            anim.SetBool("IsCrouching", isCrouching);
        }
    }

    // Function para sa Mobile OnClick Button o Keypress
    public void ToggleCrouch()
    {
        isCrouching = !isCrouching;

        // I-adjust ang Character Controller Height para kasya sa maliliit na passage
        if (controller != null)
        {
            controller.height = isCrouching ? crouchingHeight : standingHeight;
            controller.center = isCrouching ? crouchingCenter : standingCenter;
        }
    }
}