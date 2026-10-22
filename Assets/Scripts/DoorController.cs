using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private float openAngle = 90f;          
    [SerializeField] private float openSpeed = 3f;           
    [SerializeField] private GameObject interactPromptText; 

    [Header("Key Requirement Settings")]
    [SerializeField] private bool requiresKeys = false;     // CHECKBOX: I-check LANG ito sa Final Door!
    [SerializeField] private int keysNeeded = 3;             

    private bool isPlayerNearby = false;
    private bool isOpen = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    private void Start()
    {
        closedRotation = transform.localRotation;
        openRotation = Quaternion.Euler(transform.localEulerAngles + new Vector3(0, openAngle, 0));

        if (interactPromptText != null)
        {
            interactPromptText.SetActive(false);
        }
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            ToggleDoor();
        }

        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * openSpeed);
    }

    public void ToggleDoor()
    {
        // Kung bukas na ang pinto, pwede laging isara
        if (isOpen)
        {
            isOpen = false;
            return;
        }

        // Kung HINDI kailangan ng keys (Regular Door) -> Buksan agad!
        if (!requiresKeys)
        {
            isOpen = true;
            return;
        }

        // Kung KAILANGAN ng keys (Final Door) -> Check keys count
        if (KeyManager.keysCollected >= keysNeeded)
        {
            isOpen = true;
            Debug.Log("Final Door Unlocked!");
        }
        else
        {
            int missing = keysNeeded - KeyManager.keysCollected;
            Debug.Log("Door Locked! Needs " + missing + " more key(s).");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            if (interactPromptText != null)
            {
                interactPromptText.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (interactPromptText != null)
            {
                interactPromptText.SetActive(false);
            }
        }
    }
}