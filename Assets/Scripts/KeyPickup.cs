using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    [SerializeField] private GameObject interactPromptText;
    private bool isPlayerNearby = false;

    private void Update()
    {
        // Keycode 'F' para sa PC testing
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            PickUpKey();
        }
    }

    public void PickUpKey()
    {
        if (KeyManager.instance != null)
        {
            KeyManager.instance.AddKey();
        }

        if (interactPromptText != null)
        {
            interactPromptText.SetActive(false);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            if (interactPromptText != null) interactPromptText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (interactPromptText != null) interactPromptText.SetActive(false);
        }
    }
}