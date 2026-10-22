using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float crouchSpeed = 4f; 
    public TextMeshProUGUI countText;
    public TextMeshProUGUI winText;  

    private Rigidbody rb;
    private int count;

    // Animation & Crouch References
    private Animator anim;
    private bool isCrouching = false;

    // Quest Variables
    private bool pcInteracted = false;
    private bool isScene3 = false;

    void Start()
    {
        Time.timeScale = 1f;
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        
        count = 0;
        SetCountText();
        if (winText != null) winText.text = "";

        if (SceneManager.GetActiveScene().name == "Scene 3" || SceneManager.GetActiveScene().buildIndex == 3)
        {
            isScene3 = true;
        }
    }

    void Update()
    {
        // Keyboard Controls (PC)
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C))
        {
            ToggleCrouch();
        }

        // Animator parameters
        if (anim != null)
        {
            anim.SetBool("IsCrouching", isCrouching);
        }
    }

    // --- FUNCTION PARA SA MOBILE BUTTON ---
    public void ToggleCrouch()
    {
        isCrouching = !isCrouching;
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

        float currentSpeed = isCrouching ? crouchSpeed : speed;

        rb.linearVelocity = movement * currentSpeed + new Vector3(0, rb.linearVelocity.y, 0);

        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 15f * Time.fixedDeltaTime);
        }

        if (anim != null)
        {
            anim.SetFloat("speed", movement.magnitude);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }

        if (isScene3)
        {
            if (other.gameObject.CompareTag("Computer"))
            {
                if (!pcInteracted)
                {
                    pcInteracted = true;
                    if (winText != null) 
                    {
                        winText.text = "Objective: Find the Green Mesh!";
                    }
                }
            }

            if (other.gameObject.CompareTag("GreenMesh"))
            {
                if (pcInteracted)
                {
                    other.gameObject.SetActive(false);
                    
                    if (winText != null) 
                    {
                        winText.text = "Door Unlocked!";
                    }

                    GameObject doors = GameObject.Find("doors");
                    if (doors != null)
                    {
                        doors.SetActive(false);
                    }
                }
                else
                {
                    if (winText != null)
                    {
                        winText.text = "Interact with the computer first!";
                    }
                }
            }
        }
    }

    void SetCountText()
    {
        if (countText != null)
        {
            countText.text = "Count: " + count.ToString();
        }

        if (!isScene3 && count >= 9)
        {
            if (winText != null) winText.text = "You Win!";
            Invoke("NextLevel", 2f);
        }
    }

    void NextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}