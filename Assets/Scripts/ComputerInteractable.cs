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
            ToggleComputerUI();
        }

        // Saraduhan ang UI kung pindutin ang Escape habang bukas
        if (computerSystemPanel != null && computerSystemPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseComputerUI();
        }
    }

    // CALL THIS FUNCTION FROM THE ON CLICK () EVENT OF YOUR MOBILE 'E' BUTTON
    public void OnInteractButtonPressed()
    {
        if (isPlayerNearby)
        {
            ToggleComputerUI();
        }
    }

    private void ToggleComputerUI()
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

        // NOT PAUSING GAME TIME ANYMORE (Pinanatiling 1f para tuloy ang laro)
        Time.timeScale = 1f;

        // FOCUS SA INPUT FIELD PARA MAKAPAG-TYPE AGAD
        if (pinInputField != null)
        {
            pinInputField.Select();
            pinInputField.ActivateInputField();
        }

        // TAWAG SA TUTORIAL MANAGER
        TutorialManager tutorial = Object.FindFirstObjectByType<TutorialManager>();
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

        // RESUME / KEEP GAME RUNNING NORMALLY
        Time.timeScale = 1f;

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

            // =========================================================
            // TAWAGIN ANG SCENE1QUEST PARA MAGBAWAS NG 20 SECONDS SA TIMER
            // =========================================================
            Scene1Quest questScript = Object.FindFirstObjectByType<Scene1Quest>();
            if (questScript != null)
            {
                questScript.OnWrongPassword(); // Bawas 20s sa Timer
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
            
            // Isara ang UI kapag umalis ang player
            CloseComputerUI();

            if (interactPromptText != null) interactPromptText.SetActive(false);
        }
    }
}