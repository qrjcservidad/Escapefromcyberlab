using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator anim;
    private CharacterController controller;

    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float gravity = -20f; // Mabilis na gravity para laging nakadikit

    private Vector3 velocity;

    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Pag-check kung grounded
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -5f; // Constant downward force para hindi umangat ang paa
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(horizontal, 0f, vertical).normalized;

        bool isRunning = Input.GetKey(KeyCode.LeftShift) && move.magnitude > 0;
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        if (move.magnitude > 0)
        {
            // Smooth turning sa direksyon ng paglalakad
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            controller.Move(move * currentSpeed * Time.deltaTime);
        }

        // Apply gravity every frame
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Animation control
        float animSpeedParam = 0f;
        if (move.magnitude > 0)
        {
            animSpeedParam = isRunning ? 2.0f : 1.0f;
        }

        anim.SetFloat("speed", animSpeedParam);
    }
}