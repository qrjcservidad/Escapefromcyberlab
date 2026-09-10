using UnityEngine;

public class ComputerInteractable : MonoBehaviour
{
    [Header("UI References")]
    public GameObject interactPromptText; // Lagay dito ang "Press E to Interact" UI
    public GameObject computerSystemPanel; // Lagay dito ang main Computer UI Panel

    private bool isPlayerNearby = false;

    private void Start()
    {
        // Siguraduhing nakatago ang mga UI sa simula
        if (interactPromptText != null) interactPromptText.SetActive(false);
        if (computerSystemPanel != null) computerSystemPanel.SetActive(false);
    }

    private void Update()
    {
        // Gagana lang ang E kung malapit ang player sa computer
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            OpenComputerUI();
        }
    }

    public void OpenComputerUI()
    {
        if (computerSystemPanel != null)
        {
            computerSystemPanel.SetActive(true);

            // Itatago ang "Press E" prompt kapag nakabukas na ang computer panel
            if (interactPromptText != null) 
            {
                interactPromptText.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Siguraduhing may Tag na "Player" ang iyong Player GameObject
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            if (interactPromptText != null && !computerSystemPanel.activeSelf)
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
            
            // Itatago ang prompt at isasarado ang UI kapag umalis ang player
            if (interactPromptText != null) interactPromptText.SetActive(false);
            if (computerSystemPanel != null) computerSystemPanel.SetActive(false);
        }
    }
}