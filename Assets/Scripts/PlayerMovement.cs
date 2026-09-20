using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator anim;
    private CharacterController controller;
    private Transform cameraTransform;

    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float gravity = -20f;

    private Vector3 velocity;

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

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Camera direction vectors
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = (camForward * vertical + camRight * horizontal).normalized;

        bool isRunning = Input.GetKey(KeyCode.LeftShift) && move.magnitude > 0;
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        if (move.magnitude > 0)
        {
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            controller.Move(move * currentSpeed * Time.deltaTime);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        float animSpeedParam = 0f;
        if (move.magnitude > 0)
        {
            animSpeedParam = isRunning ? 2.0f : 1.0f;
        }

        anim.SetFloat("speed", animSpeedParam);
    }
}