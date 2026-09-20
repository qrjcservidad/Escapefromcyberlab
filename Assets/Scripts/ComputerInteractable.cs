using UnityEngine;
using TMPro; // kailangan para sa TextMeshPro UI elements

public class ComputerInteractable : MonoBehaviour
{
    [Header("UI References")]
    public GameObject interactPromptText; // "Press E to Interact" UI
    public GameObject computerSystemPanel; // Main Computer UI Panel

    [Header("Terminal PIN Settings")]
    public TMP_InputField pinInputField;   // Slot para sa PinInput
    public TextMeshProUGUI statusText;     // Slot para sa FeedbackText
    public string correctPin = "1326";     // Ang tamang PIN
    
    [Header("UI Panels Transition")]
    public GameObject pinGroup;            // Lagay dito ang PIN Input Field + Proceed Button
    public GameObject quizOptionsGroup;    // Lagay dito ang 4 na Quiz Options/Choices

    private bool isPlayerNearby = false;

    private void Start()
    {
        // Siguraduhing nakatago ang mga UI sa simula
        if (interactPromptText != null) interactPromptText.SetActive(false);
        if (computerSystemPanel != null) computerSystemPanel.SetActive(false);
        
        // I-hide muna ang Quiz Options sa simula
        if (quizOptionsGroup != null) quizOptionsGroup.SetActive(false);
        
        // Siguraduhing nakalabas muna ang PIN group kapag nag-open
        if (pinGroup != null) pinGroup.SetActive(true);
    }

    private void Update()
    {
        // Toggle or open computer UI kapag pinindot ang E habang malapit
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (computerSystemPanel != null && computerSystemPanel.activeSelf)
            {
                CloseComputerUI();
            }
            else
            {
                OpenComputerUI();
            }
        }

        // Saraduhan ang UI kung pindutin ang Escape habang bukas
        if (computerSystemPanel != null && computerSystemPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseComputerUI();
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

        // 1. PAUSE GAME TIME & PLAYER CONTROLS
        Time.timeScale = 0f;

        // 2. UNLOCK AT IPAKITA ANG CURSOR PARA SA UI INTERACTION
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 3. FOCUS SA INPUT FIELD PARA MAKAPAG-TYPE AGAD
        if (pinInputField != null)
        {
            pinInputField.Select();
            pinInputField.ActivateInputField();
        }

        // TAWAG SA TUTORIAL MANAGER
        TutorialManager tutorial = FindFirstObjectByType<TutorialManager>();
        if (tutorial != null)
        {
            tutorial.CompleteTerminalTask();
        }
    }

    public void CloseComputerUI()
    {
        if (computerSystemPanel != null)
        {
            computerSystemPanel.SetActive(false);
        }

        // 1. RESUME GAME TIME & PLAYER CONTROLS
        Time.timeScale = 1f;

        // 2. IBATIN ANG CURSOR SA LOCKED STATE PARA SA CAMERA CONTROL
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Ipakita muli ang "Press E" prompt kung malapit pa ang player
        if (isPlayerNearby && interactPromptText != null)
        {
            interactPromptText.SetActive(true);
        }
    }

    // TAWAGAN 'TO SA ON CLICK () NG PROCEED BUTTON
    public void VerifyPin()
    {
        if (pinInputField == null) return;

        if (pinInputField.text == correctPin)
        {
            if (statusText != null)
            {
                statusText.text = "ACCESS GRANTED";
                statusText.color = Color.green;
            }

            // 1. ITATAGO ANG PIN INPUT FIELD AT PROCEED BUTTON
            if (pinGroup != null) 
            {
                pinGroup.SetActive(false);
            }

            // 2. LALABAS NA ANG 4 NA SASAGUTAN / QUIZ CHOICES
            if (quizOptionsGroup != null) 
            {
                quizOptionsGroup.SetActive(true);
            }
        }
        else
        {
            if (statusText != null)
            {
                statusText.text = "INCORRECT PIN";
                statusText.color = Color.red;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            
            bool isPanelOpen = computerSystemPanel != null && computerSystemPanel.activeSelf;

            if (interactPromptText != null && !isPanelOpen)
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
            
            // Isara ang UI at ibalik ang game time/cursor sa normal kapag umalis ang player
            CloseComputerUI();

            if (interactPromptText != null) interactPromptText.SetActive(false);
        }
    }
}